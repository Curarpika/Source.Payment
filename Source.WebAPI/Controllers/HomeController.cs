using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.TenPay.V3;
using Source.Auth.Models;
using Source.Auth.Services;
using Source.Payment;
using Source.Payment.Models;
using Source.Payment.Models.Enums;
using Source.Payment.Services;
using Source.WebAPI.Filters;
using Senparc.Weixin;
using Source.Product.Models;
using Source.Product.Services;
using Source.Product.Models.Enums;

namespace Source.WebAPI
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IMqttServer _mqttServer;
        private readonly IAuthService _authSrv;
        private readonly IPaymentService _paySrv;
        
        private readonly IProductService _prdSrv;
        private readonly IProductOrderService _orderSrv;
        private static TenPayV3Info _tenPayV3Info;
        private readonly Payment.Models.Business _biz;
        private readonly string _siteUrl; 
        public HomeController(IConfiguration config,
         IMqttServer mqttServer,
         IPaymentService paySrv,
         IProductService prdSrv,
         IProductOrderService orderSrv,
          IAuthService authSrv)
        {
            _config = config;
            _authSrv = authSrv;
            _paySrv = paySrv;
            _prdSrv = prdSrv;
            _orderSrv = orderSrv;
            _biz = new Payment.Models.Business();
            config.GetSection("Business").Bind(_biz);
            _siteUrl = config.GetSection("SiteUrl").Value;

            _mqttServer = mqttServer;
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

        // public async Task<IActionResult> Test()
        // {
        //     var factory = new MqttFactory();
        //     var mqttClient = factory.CreateMqttClient();
        //     var mqttOptions = new MqttClientOptionsBuilder()
        //             .WithClientId("1fasdfioifiagsf")
        //             .WithKeepAlivePeriod(TimeSpan.FromHours(24))
        //             .WithKeepAliveSendInterval(TimeSpan.FromSeconds(5))
        //             .WithCleanSession()
        //             .WithWebSocketServer("127.0.0.1/mqtt")
        //             .Build();
        //     await mqttClient.ConnectAsync(mqttOptions, new System.Threading.CancellationToken());

        //     var serializerSettings = new JsonSerializerSettings();
        //     serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //     var json = JsonConvert.SerializeObject(_paySrv.GetPaidOrders().FirstOrDefault(), serializerSettings);

        //     await _mqttServer.PublishAsync("PaidOrders", json);
        //     return View();
        // }

        public async Task<IActionResult> Index()
        {

            return View(_biz);
        }

        [CustomOAuth(null, "/WxPay/OAuthCallback")]
        public async Task<IActionResult> WxPayIndex()
        {
            // 获取openId
            var openId = HttpContext.Session.GetString("OpenId");
            if(string.IsNullOrEmpty(openId))
            {
                return Json(Url.Action("Index", "Home"));
            }
            var accessToken = HttpContext.Session.GetString("accessToken");

            HttpContext.Session.SetString("SiteUrl", _siteUrl);

            // 根据openId 查询用户
            var user = _authSrv.GetUserByExternalId(openId, 1);

            // 获取微信用户信息
            OAuthUserInfo userInfo = OAuthApi.GetUserInfo(accessToken, openId);

            // 用户存在则获取信息，不存在则创建用户
            if (user == null)
            {
                var newUser = new BaseUser { UserName = openId, ExternalId = openId, ExternalType = 1, ExternalName = userInfo.nickname };
                var result = await _authSrv.CreateUser(newUser, openId);

                if (result.Succeeded)
                {
                    var role = await _authSrv.AddRolesAsync(openId, new string[] { "SysUser" });
                }
                user = newUser;

            }
            var paidOrders = _orderSrv.GetPaidOrders().Where(q => (q.OrderType == OrderType.BuyInStore ||q.OrderType == OrderType.BuyOnline)  && q.BuyerIdentity == openId);

            // Card
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(TenPayV3Info.AppId, TenPayV3Info.AppSecret, Request.AbsoluteUri());
            var api_ticket = WxCardApiTicketContainer.TryGetWxCardApiTicket(TenPayV3Info.AppId, TenPayV3Info.AppSecret);

            // var openId = HttpContext.Session.GetString("OpenId");
            var timeStamp = TenPayV3Util.GetTimestamp();
            var nonceStr = TenPayV3Util.GetNoncestr();
            var sign = JSSDKHelper.GetcardExtSign(api_ticket, timeStamp, "pukHe541WmaHEBgW3gACiBCD4EbY", nonceStr);
            var cardExt = new { timestamp = timeStamp, signature = sign, nonce_str = nonceStr };

            // 登陆Identity用户
            await _authSrv.Login(user);
            ViewData["credit"] = user.Credit;
            ViewData["biz"] = _biz;
            ViewData["userInfo"] = userInfo;
            ViewData["paidOrders"] = paidOrders;
            ViewData["jssdkUiPackage"] = jssdkUiPackage;
            ViewData["cardExt"] = JsonConvert.SerializeObject(cardExt);

            return View();
            // 前端菜单：直接支付，跳转js支付，余额支付跳转/Home/CreditPay，套餐支付：
        }

        public async Task<IActionResult> WxPayBuy()
        {
            try
            {
                ViewData["biz"] = _biz;
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public async Task<IActionResult> WxPayOrder(Guid orderId)
        {
            try
            {
                var order = _paySrv.GetPaymentOrderById(orderId);
                return View(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomOAuth(null, "/AliPay/OAuthCallback")]
        public async Task<IActionResult> AlipayIndex()
        {
            // 获取openId
            var openId = HttpContext.Session.GetString("OpenId");
            if(string.IsNullOrEmpty(openId))
            {
                return Json(Url.Action("Index", "Home"));
            }
            var type = HttpContext.Session.GetString("IdType");

            // 根据openId 查询用户
            var user = _authSrv.GetUserByExternalId(openId, 0);

            // 用户存在则获取信息，不存在则创建用户

            // 返回用户信息

            // 前端菜单：直接支付，跳转js支付，余额支付跳转/Home/CreditPay，套餐支付：

            return View();
        }

        [CustomOAuth(null, "/AliPay/OAuthCallback")]
        [HttpPost("/Home/CreateOrder")]
        public async Task<IActionResult> CreateOrder(string id, int quantity, OrderType orderType, [FromBody]CartInfo[] carts)
        {
            try
            {
                var openId = HttpContext.Session.GetString("OpenId");
                if(string.IsNullOrEmpty(openId))
                {
                    return Json(Url.Action("Index", "Home"));
                }
                //获取产品信息
                var product = _biz.Products.FirstOrDefault(z => z.Id == id);
                if (product == null)
                {
                    return Content("商品信息不存在，或非法进入！1002");
                }
                
                var order = new ProductOrder();

                // 线上自助餐厅业务，如果消费选择是自助餐，且购物车有数据
                if(carts != null && product.ProductType == "Product" && orderType == OrderType.BuyOnline)
                {
                    var prds = new List<CartInfo>();
                    foreach (var c in carts)
                    {
                        var cart = _prdSrv.GetProductByIds(c.CartItems.Select(x=>x.ProductId).ToArray()).Select(x=> new CartItem {Product = x, Quantity = c.CartItems.Where(cc=>cc.ProductId == x.Id).FirstOrDefault().Quantity}).ToList();
                        prds.Add(new CartInfo(){ CartItems = cart, Amount = cart.Sum(x=>x.Product.Price)});
                    }
                    order.Snapshot = JsonConvert.SerializeObject(prds);
                }

                // 自助餐厅，固定价格
                var amount = product.Amount * quantity;

                order.Quantity = quantity;
                order.Content = product.Name;                
                order.Amount = amount;
                order.OrderType = product.ProductType == "Credit" ? OrderType.AddCredit : orderType;
                order.BuyerIdentity = openId;
                order.OrderState = Product.Models.Enums.OrderState.Ordered;

                // 外卖核销地址
                if(orderType == OrderType.BuyOnline)
                {
                    var user = await _authSrv.GetUserInfo(openId);
                    order.ExcuteAddress =user.Address;
                    order.Contact = $"{user.ExternalName} {user.PhoneNumber}";
                }
                _orderSrv.CreateProductOrder(order);
                return Json(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomOAuth(null, "/AliPay/OAuthCallback")]
        [HttpPost("/Home/CreatePaymentOrder")]
        public async Task<IActionResult> CreatePaymentOrder(Guid orderId, PayMethod method)
        {
                var openId = HttpContext.Session.GetString("OpenId");
                if(string.IsNullOrEmpty(openId))
                {
                    return Json(Url.Action("Index", "Home"));
                }
                var order = _orderSrv.GetProductOrderById(orderId);
                if(order == null)
                {
                    return Json(Url.Action("ErrorOrder", "Home", new { message = "订单不存在" }));
                }

                if(order.OrderType == OrderType.AddCredit && method == PayMethod.Credit)
                {
                    return Json(Url.Action("ErrorOrder", "Home", new { message = "该商品不支持积分购买" }));
                }

                // 创建支付订单
                var pay = new PaymentOrder();
                pay.Amount = (decimal)order.Amount;
                pay.PayMethod = method; 
                pay.UserId = openId;
                pay.OrderState = Payment.Models.Enums.OrderState.WaitForPayment;
                pay.OrderId = order.Id;
                _paySrv.CreatePaymentOrder(pay);

                // 积分支付购买商品
                if (order.OrderType != OrderType.AddCredit && method == PayMethod.Credit)
                {
                    var result = await CreditPay(pay);
                    return Json(Url.Action("WxPayOrder", "WxPay", new { orderid = order.Id, payid = pay.Id}));
                }
                // 微信支付
                else if (method == PayMethod.Wechat)
                {
                    return Json(Url.Action("WxPayOrder", "WxPay", new { orderid = order.Id, payid = pay.Id }));
                }
                else if (method == PayMethod.Alipay)
                {
                    return Json(Url.Action("JsApi", "Alipay", new { orderid = order.Id, payid = pay.Id }));
                }
                else
                {
                    return Json(Url.Action("ErrorOrder", "Home", new { message = "支付订单错误" }));
                }
        }

        [HttpPost("/Home/ProcessOrder")]
        public async Task<IActionResult> ProcessOrder(Guid orderId)
        {       
            var processedOrder = _orderSrv.UpdateProductOrder(orderId, Product.Models.Enums.OrderState.Excuted);
            return Json(processedOrder);
        }

        [HttpPost("/Home/UpdateContact")]
        public async Task<IActionResult> UpdateContact(string address, string phone)
        {
            if(string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phone))
            {
                return BadRequest();
            }
            var openId = HttpContext.Session.GetString("OpenId");
            if(string.IsNullOrEmpty(openId))
            {
                return Json(Url.Action("Index", "Home"));
            }
            var user = _authSrv.GetUserByExternalId(openId, 1);
            user.Address = address;
            user.PhoneNumber = phone;
            var result = await _authSrv.UpdateUser(user);
            return Json(result);
        }

        [HttpGet("/Home/GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var openId = HttpContext.Session.GetString("OpenId");
            if(string.IsNullOrEmpty(openId))
            {
                return Json(Url.Action("Index", "Home"));
            }
            var user = _authSrv.GetUserInfo(openId);
            return Json(user);
        }

        private async Task<decimal> CreditPay(PaymentOrder pay)
        {
            var openId = HttpContext.Session.GetString("OpenId");
            if(string.IsNullOrEmpty(openId))
            {
                throw new ApplicationException("Unauthorized");
            }
            
            var type = Int16.Parse(HttpContext.Session.GetString("IdType"));

            // 扣除余额
            var result = await _authSrv.UpdateCredit(openId, false, pay.Amount);

            // 支付完成
            _paySrv.UpdatePaymentResult(pay.Id, true);

            var order = _orderSrv.GetProductOrderById((Guid)pay.OrderId);
            _orderSrv.UpdateProductOrder(order.Id, Product.Models.Enums.OrderState.Excuting, pay.Id);

            // 发送订单消息
            await SendOrder(order);

            // 返回余额
            return result;
        }

        private async Task<ActionResult> SendOrder(ProductOrder order)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(order, serializerSettings);

            var result = await _mqttServer.PublishAsync("PaidOrders", json);
            return Json(result);
        }

        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> GetPaidOrders()
        {
            var result = _orderSrv.GetPaidOrders().Where(q => q.OrderType == OrderType.BuyOnline || q.OrderType == OrderType.BuyInStore);
            return Json(result);
        }

        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> GetUserOrders()
        {
            var openId = HttpContext.Session.GetString("OpenId");
            var result = _orderSrv.GetPaidOrders().Where(q => (q.OrderType == OrderType.BuyOnline || q.OrderType == OrderType.BuyInStore) && q.BuyerIdentity == openId);
            return Json(result);
        }

        [HttpPost("/Home/Dashboard")]
        public async Task<IActionResult> Dashboard(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var label = new List<string>();
                var creditAddedValue = new List<decimal>();
                var cashUsedValue = new List<decimal>();

                var pay = _paySrv.GetPaymentOrders().Where(q => q.PaidTime >= dateStart.Date && q.PaidTime <= dateEnd.AddDays(1).Date).ToList();
                var order = _orderSrv.GetProductOrders().Where(q => q.ExecutedDatedTime >= dateStart.Date && q.ExecutedDatedTime <= dateEnd.AddDays(1).Date).ToList();
                var creditAdded = order.Where(x => x.OrderType == OrderType.AddCredit);
                var creditUsed = pay.Where(x =>  x.PayMethod == PayMethod.Credit);
                var cashUsed = pay.Where(x => x.PayMethod != PayMethod.Credit);

                // 当日
                if (dateStart == dateEnd)
                {
                    var hour = 0;
                    while (hour < 24)
                    {
                        label.Add($"{hour.ToString().PadLeft(2, '0')}:00");
                        creditAddedValue.Add(creditAdded.Where(q => q.ExecutedDatedTime.Value.Date == dateStart.Date && q.ExecutedDatedTime.Value.Hour == hour).Sum(s => s.Amount) ?? 0);
                        cashUsedValue.Add(cashUsed.Where(q => q.PaidTime.Value.Date == dateStart.Date && q.PaidTime.Value.Hour == hour).Sum(s => s.Amount));
                        hour = hour + 1;
                    }
                }
                // 按周、月
                else
                {
                    var time = dateStart;
                    while (time <= dateEnd)
                    {
                        label.Add(time.ToString("MM-dd"));
                        creditAddedValue.Add(creditAdded.Where(q => q.ExecutedDatedTime.Value.Date == time.Date).Sum(s => s.Amount) ?? 0);
                        cashUsedValue.Add(cashUsed.Where(q => q.PaidTime.Value.Date == time.Date).Sum(s => s.Amount));
                        time = time.AddDays(1);
                    }
                }

                var creditAddedTotal = creditAdded.Count();
                var creditUsedTotal = creditUsed.Count();
                var cashUsedTotal = cashUsed.Count();

                var result = new
                {
                    label,
                    creditAddedValue,
                    creditAddedTotal,
                    creditUsedTotal,
                    cashUsedValue,
                    cashUsedTotal
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> GetPaymentOrders(string key, int? orderType, int? payMethod, int? orderState, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var result = _paySrv.GetPaymentOrders(key, orderType, payMethod, orderState, pageIndex, pageSize);

                var total = result.Count;
                var data = result.Orders;

                return Json(new { data, total });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}