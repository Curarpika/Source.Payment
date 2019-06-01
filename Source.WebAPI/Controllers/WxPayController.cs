/*----------------------------------------------------------------
    Copyright (C) 2019 Senparc
    
    文件名：TenPayV3Controller.cs
    文件功能描述：微信支付V3Controller
    
    
    创建标识：Senparc - 20150312
 
    修改标识：Senparc - 20150419
    修改描述：添加产品相关

    修改标识：Senparc - 20161203
    修改描述：调用新版Unifiedorder方法

    修改标识：Senparc - 20161204
    修改描述：调用新版Unifiedorder方法
----------------------------------------------------------------*/

//DPBMARK_FILE TenPay
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Senparc.Weixin.BrowserUtility;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.TenPay.V3;
using ZXing;
using ZXing.Common;
using Senparc.Weixin.Exceptions;
using Microsoft.AspNetCore.Http;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using Source.WebAPI.Filters;
using Source.Payment.TemplateMessage;
using Source.WebAPI.Models;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Source.Payment;
using Microsoft.Extensions.Configuration;
using Source.Payment.Models;
using Source.Payment.Services;
using Source.Payment.Models.Enums;
using Source.Auth.Services;
using System.Threading.Tasks;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Senparc.Weixin.MP.Containers;

namespace Source.WebAPI.Controllers
{
    /* 
     * 友情提示：微信支付正式上线之前，请进行沙箱测试！ 
     * 单元测试见：Senparc.Weixin.MP.Test.TenPayV3/TenPayV3Test.cs/GetSignKeyTest()
     */


    /// <summary>
    /// 根据官方的Webforms Demo改写，所以可以看到直接Response.Write()之类的用法，实际项目中不提倡这么做。
    /// </summary>
    public class WxPayController : Controller
    {
        private static TenPayV3Info _tenPayV3Info;
        private readonly Business _biz;
        private readonly IAuthService _authSrv;
        private readonly IMqttServer _mqttServer;
        private readonly IPaymentService _paySrv;

        public WxPayController(IConfiguration config,
        IAuthService authSrv,
        IMqttServer mqttServer,
        IPaymentService paySrv)
        {
            _mqttServer = mqttServer;
            _paySrv = paySrv;
            _authSrv = authSrv;
            _biz = new Business();
            config.GetSection("Business").Bind(_biz);
        }

        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    var key = TenPayV3InfoCollection.GetKey(Config.SenparcWeixinSetting);

                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[key];
                }
                return _tenPayV3Info;
            }
        }
        public async Task<ActionResult> Test()
        {
            //注册
            await AccessTokenContainer.RegisterAsync(TenPayV3Info.AppId, TenPayV3Info.Sub_AppSecret);
            
            var accessToken = AccessTokenContainer.TryGetAccessToken("wx4b58f9e96371d9ff", "43c5c4d9b8207634eb293a72ddad5bb1",true);

            var token = await AccessTokenContainer.TryGetAccessTokenAsync(TenPayV3Info.AppId, TenPayV3Info.Sub_AppSecret, false);

            //获取Ticket完整结果（包括当前过期秒数）
            var ticketResult = JsApiTicketContainer.GetJsApiTicketResult(TenPayV3Info.AppId);

            // //只获取Ticket字符串
            var ticket = JsApiTicketContainer.GetJsApiTicket(TenPayV3Info.AppId);

            //getNewTicket
            {
                ticket = JsApiTicketContainer.TryGetJsApiTicket(TenPayV3Info.AppId,  TenPayV3Info.Sub_AppSecret, false);


                ticket = JsApiTicketContainer.TryGetJsApiTicket(TenPayV3Info.AppId, TenPayV3Info.Sub_AppSecret, true);
                //Assert.AreNotEqual(ticketResult.ticket, ticket);//如果微信服务器缓存，此处会相同
            }
            return Content(token);
        }
        
        /// <summary>
        /// 获取用户的OpenId
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string id)
        {
            if (id == null)
            {
                return RedirectToAction("ProductList");
            }

            var returnUrl = $"http://payment.gaodev.com/WxPay/JsApi?id={id}";
            var state = id;
            var url = OAuthApi.GetAuthorizeUrl(TenPayV3Info.AppId, returnUrl, state, OAuthScope.snsapi_userinfo);

            return Redirect(url);
        }

        #region JsApi支付
        //需要OAuth登录
        [CustomOAuth(null, "/WxPay/OAuthCallback")]
        public ActionResult WxPayOrder(Guid orderid)
        {
            try
            {
                var openId = HttpContext.Session.GetString("OpenId");

                var order = _paySrv.GetPaymentOrderById(orderid);
                if (order == null)
                {
                    return NotFound();
                }
                string sp_billno = GuidEncoder.Encode(order.Id);
                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();

                var body = orderid.ToString();
                var price = (int)(order.Amount * 100);
                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(TenPayV3Info.AppId, TenPayV3Info.MchId, body, sp_billno, price, HttpContext.UserHostAddress()?.ToString(), TenPayV3Info.TenPayV3Notify, Senparc.Weixin.TenPay.TenPayV3Type.JSAPI, openId, TenPayV3Info.Key, nonceStr);

                var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口
                                                                //JsSdkUiPackage jsPackage = new JsSdkUiPackage(TenPayV3Info.AppId, timeStamp, nonceStr,);
                var package = string.Format("prepay_id={0}", result.prepay_id);

                ViewData["appId"] = TenPayV3Info.AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = package;
                ViewData["paySign"] = TenPayV3.GetJsPaySign(TenPayV3Info.AppId, timeStamp, nonceStr, package, TenPayV3Info.Key);
                ViewData["successUrl"] = Url.Action("WxPayOrder", "WxPay", new { orderid = orderid });

                //临时记录订单信息，留给退款申请接口测试使用
                HttpContext.Session.SetString("BillNo", sp_billno);
                HttpContext.Session.SetString("BillFee", price.ToString());

                return View(order);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);
            }
        }

        public ActionResult OAuthCallback(string code, string state, string returnUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Content("您拒绝了授权！");
                }

                if (!state.Contains("|"))
                {
                    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                    //实际上可以存任何想传递的数据，比如用户ID
                    return Content("验证失败！请从正规途径进入！1001");
                }

                //通过，用code换取access_token
                var openIdResult = OAuthApi.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    return Content("错误：" + openIdResult.errmsg);
                }

                HttpContext.Session.SetString("OpenId", openIdResult.openid);//进行登录
                HttpContext.Session.SetString("accessToken", openIdResult.access_token);//进行登录
                HttpContext.Session.SetString("IdType", "1");//进行登录

                //也可以使用FormsAuthentication等其他方法记录登录信息，如：
                //FormsAuthentication.SetAuthCookie(openIdResult.openid,false);

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }

        }

        private async Task<bool> Paid(Guid orderid, bool succeed)
        {
            var order = _paySrv.UpdatePaymentResult(orderid, succeed);

            if (order.OrderState == OrderState.Paid && order.OrderType == OrderType.AddCredit)
            {
                var user = _authSrv.GetUserByExternalId(order.UserId, 1);
                if (user == null)
                    return false;
                await _authSrv.UpdateCredit(user.ExternalId, true, order.Quantity);
                _paySrv.ProcessPaymentOrder(orderid);
            }

            if (order.OrderState == OrderState.Paid && order.OrderType == OrderType.Buy)
            {
                await SendOrder(order);
            }

            return true;
        }

        private async Task<ActionResult> SendOrder(PaymentOrder order)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(order, serializerSettings);

            var result = await _mqttServer.PublishAsync("PaidOrders", json);
            return Json(result);
        }

        /// <summary>
        /// JS-SDK支付回调地址（在统一下单接口中设置notify_url）
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PayNotifyUrl(string id)
        {
            try
            {
                ResponseHandler resHandler = new ResponseHandler(HttpContext);

                string return_code = resHandler.GetParameter("return_code");
                string return_msg = resHandler.GetParameter("return_msg");

                string res = null;

                resHandler.SetKey(TenPayV3Info.Key);
                string out_trade_no = resHandler.GetParameter("out_trade_no");

                //TODO id for debug
                var guid = GuidEncoder.Decode(out_trade_no ?? id);

                //验证请求是否从微信发过来（安全）
                if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
                {
                    res = "success";//正确的订单处理
                    //直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！
                    // TODO

                    await Paid(guid, true);
                }
                else
                {
                    res = "wrong";//错误的订单处理
                    await Paid(guid, true);
                }

                /* 这里可以进行订单处理的逻辑 */

                //发送支付成功的模板消息
                try
                {
                    string appId = Config.SenparcWeixinSetting.TenPayV3_AppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
                    string openId = resHandler.GetParameter("openid");
                    var templateData = new WeixinTemplate_PaySuccess("http://payment.gaodev.com", "购买商品", "状态：" + return_code);

                    Senparc.Weixin.WeixinTrace.SendCustomLog("支付成功模板消息参数", appId + " , " + openId);

                    var result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(appId, openId, templateData);
                }
                catch (Exception ex)
                {
                    Senparc.Weixin.WeixinTrace.SendCustomLog("支付成功模板消息", ex.ToString());
                }

                #region 记录日志

                var logDir = ServerUtility.ContentRootMapPath(string.Format("~/App_Data/TenPayNotify/{0}", SystemTime.Now.ToString("yyyyMMdd")));
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                var logPath = Path.Combine(logDir, string.Format("{0}-{1}-{2}.txt", SystemTime.Now.ToString("yyyyMMdd"), SystemTime.Now.ToString("HHmmss"), Guid.NewGuid().ToString("n").Substring(0, 8)));

                using (var fileStream = System.IO.File.OpenWrite(logPath))
                {
                    var notifyXml = resHandler.ParseXML();
                    //fileStream.Write(Encoding.Default.GetBytes(res), 0, Encoding.Default.GetByteCount(res));

                    fileStream.Write(Encoding.Default.GetBytes(notifyXml), 0, Encoding.Default.GetByteCount(notifyXml));
                    fileStream.Close();
                }

                #endregion


                string xml = string.Format(@"<xml>
<return_code><![CDATA[{0}]]></return_code>
<return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);
                return Content(xml, "text/xml");
            }
            catch (Exception ex)
            {
                WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));
                throw;
            }
        }

        #endregion

        #region 订单及退款

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderQuery()
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("transaction_id", "");       //填入微信订单号
            packageReqHandler.SetParameter("out_trade_no", "");         //填入商家订单号
            packageReqHandler.SetParameter("nonce_str", nonceStr);             //随机字符串
            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.OrderQuery(data);
            var res = XDocument.Parse(result);
            string openid = res.Element("xml").Element("sign").Value;

            return Content(openid);
        }

        /// <summary>
        /// 关闭订单接口
        /// </summary>
        /// <returns></returns>
        public ActionResult CloseOrder()
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("out_trade_no", "");                 //填入商家订单号
            packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.CloseOrder(data);
            var res = XDocument.Parse(result);
            string openid = res.Element("xml").Element("openid").Value;

            return Content(openid);
        }

        /// <summary>
        /// 退款申请接口
        /// </summary>
        /// <returns></returns>
        public ActionResult Refund()
        {
            try
            {
                WeixinTrace.SendCustomLog("进入退款流程", "1");

                string nonceStr = TenPayV3Util.GetNoncestr();

                string outTradeNo = HttpContext.Session.GetString("BillNo");

                WeixinTrace.SendCustomLog("进入退款流程", "2 outTradeNo：" + outTradeNo);


                string outRefundNo = "OutRefunNo-" + SystemTime.Now.Ticks;
                int totalFee = int.Parse(HttpContext.Session.GetString("BillFee"));
                int refundFee = totalFee;
                string opUserId = TenPayV3Info.MchId;
                var notifyUrl = "http://payment.gaodev.com/TenPayV3/RefundNotifyUrl";
                var dataInfo = new TenPayV3RefundRequestData(TenPayV3Info.AppId, TenPayV3Info.MchId, TenPayV3Info.Key,
                    null, nonceStr, null, outTradeNo, outRefundNo, totalFee, refundFee, opUserId, null, notifyUrl: notifyUrl);

                #region 旧方法
                //var cert = @"D:\cert\apiclient_cert_SenparcRobot.p12";//根据自己的证书位置修改
                //var password = TenPayV3Info.MchId;//默认为商户号，建议修改
                //var result = TenPayV3.Refund(dataInfo, cert, password);
                #endregion

                #region 新方法（Senparc.Weixin v6.4.4+）
                var result = TenPayV3.Refund(dataInfo);//证书地址、密码，在配置文件中设置，并在注册微信支付信息时自动记录
                #endregion

                WeixinTrace.SendCustomLog("进入退款流程", "3 Result：" + result.ToJson());


                return Content(string.Format("退款结果：{0} {1}。您可以刷新当前页面查看最新结果。", result.result_code, result.err_code_des));
                //return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));

                throw;
            }


            #region 原始方法

            //RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            //packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		 //公众账号ID
            //packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);	     //商户号
            //packageReqHandler.SetParameter("out_trade_no", "124138540220170502163706139412"); //填入商家订单号
            ////packageReqHandler.SetParameter("out_refund_no", "");                //填入退款订单号
            //packageReqHandler.SetParameter("total_fee", "");                    //填入总金额
            //packageReqHandler.SetParameter("refund_fee", "100");                //填入退款金额
            //packageReqHandler.SetParameter("op_user_id", TenPayV3Info.MchId);   //操作员Id，默认就是商户号
            //packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
            //string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            //packageReqHandler.SetParameter("sign", sign);	                    //签名
            ////退款需要post的数据
            //string data = packageReqHandler.ParseXML();

            ////退款接口地址
            //string url = "http://api.mch.weixin.qq.com/secapi/pay/refund";
            ////本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
            //string cert = @"D:\cert\apiclient_cert_SenparcRobot.p12";
            ////私钥（在安装证书时设置）
            //string password = TenPayV3Info.MchId;
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ////调用证书
            //X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            //#region 发起post请求
            //HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            //webrequest.ClientCertificates.Add(cer);
            //webrequest.Method = "post";

            //byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
            //webrequest.ContentLength = postdatabyte.Length;
            //Stream stream;
            //stream = webrequest.GetRequestStream();
            //stream.Write(postdatabyte, 0, postdatabyte.Length);
            //stream.Close();

            //HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
            //StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            //string responseContent = streamReader.ReadToEnd();
            //#endregion

            //// var res = XDocument.Parse(responseContent);
            ////string openid = res.Element("xml").Element("out_refund_no").Value;
            //return Content("申请成功：<br>" + HttpUtility.RequestUtility.HtmlEncode(responseContent));

            #endregion

        }

        /// <summary>
        /// 退款通知地址
        /// </summary>
        /// <returns></returns>
        public ActionResult RefundNotifyUrl()
        {
            WeixinTrace.SendCustomLog("RefundNotifyUrl被访问", "IP" + HttpContext.UserHostAddress()?.ToString());

            string responseCode = "FAIL";
            string responseMsg = "FAIL";
            try
            {
                ResponseHandler resHandler = new ResponseHandler(null);

                string return_code = resHandler.GetParameter("return_code");
                string return_msg = resHandler.GetParameter("return_msg");

                WeixinTrace.SendCustomLog("跟踪RefundNotifyUrl信息", resHandler.ParseXML());

                if (return_code == "SUCCESS")
                {
                    responseCode = "SUCCESS";
                    responseMsg = "OK";

                    string appId = resHandler.GetParameter("appid");
                    string mch_id = resHandler.GetParameter("mch_id");
                    string nonce_str = resHandler.GetParameter("nonce_str");
                    string req_info = resHandler.GetParameter("req_info");

                    var decodeReqInfo = TenPayV3Util.DecodeRefundReqInfo(req_info, TenPayV3Info.Key);
                    var decodeDoc = XDocument.Parse(decodeReqInfo);

                    //获取接口中需要用到的信息
                    string transaction_id = decodeDoc.Root.Element("transaction_id").Value;
                    string out_trade_no = decodeDoc.Root.Element("out_trade_no").Value;
                    string refund_id = decodeDoc.Root.Element("refund_id").Value;
                    string out_refund_no = decodeDoc.Root.Element("out_refund_no").Value;
                    int total_fee = int.Parse(decodeDoc.Root.Element("total_fee").Value);
                    int? settlement_total_fee = decodeDoc.Root.Element("settlement_total_fee") != null
                            ? int.Parse(decodeDoc.Root.Element("settlement_total_fee").Value)
                            : null as int?;
                    int refund_fee = int.Parse(decodeDoc.Root.Element("refund_fee").Value);
                    int tosettlement_refund_feetal_fee = int.Parse(decodeDoc.Root.Element("settlement_refund_fee").Value);
                    string refund_status = decodeDoc.Root.Element("refund_status").Value;
                    string success_time = decodeDoc.Root.Element("success_time").Value;
                    string refund_recv_accout = decodeDoc.Root.Element("refund_recv_accout").Value;
                    string refund_account = decodeDoc.Root.Element("refund_account").Value;
                    string refund_request_source = decodeDoc.Root.Element("refund_request_source").Value;

                    //进行业务处理
                }
            }
            catch (Exception ex)
            {
                responseMsg = ex.Message;
                WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));
            }

            string xml = string.Format(@"<xml>
<return_code><![CDATA[{0}]]></return_code>
<return_msg><![CDATA[{1}]]></return_msg>
</xml>", responseCode, responseMsg);
            return Content(xml, "text/xml");
        }

        #endregion

        public IActionResult Card()
        {
            try
            {
                var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(TenPayV3Info.AppId, TenPayV3Info.AppSecret, Request.AbsoluteUri());

                var api_ticket = WxCardApiTicketContainer.TryGetWxCardApiTicket(TenPayV3Info.AppId, TenPayV3Info.AppSecret);
                // var openId = HttpContext.Session.GetString("OpenId");
                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();
                var sign = JSSDKHelper.GetcardExtSign(api_ticket, timeStamp, "pukHe541WmaHEBgW3gACiBCD4EbY", nonceStr);
                var cardExt = new { timestamp = timeStamp, signature = sign, nonce_str = nonceStr};
                ViewData["cardExt"] = JsonConvert.SerializeObject(cardExt);
                return View(jssdkUiPackage);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);
            }
        }
    }
}
