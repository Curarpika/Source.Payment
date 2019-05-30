using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using Source.Auth.Models;
using Source.Auth.Services;
using Source.Payment;
using Source.Payment.Models;
using Source.Payment.Models.Enums;
using Source.Payment.Services;
using Source.WebAPI.Filters;

namespace Source.WebAPI
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IMqttServer _mqttServer;
        private readonly IAuthService _authSrv;
        private readonly IPaymentService _paySrv;
        private readonly Business _biz;
        public HomeController(IConfiguration config,
         IMqttServer mqttServer,
         IPaymentService paySrv,
          IAuthService authSrv)
        {
            _config = config;
            _authSrv = authSrv;
            _paySrv = paySrv;
            _biz = new Business();
            config.GetSection("Business").Bind(_biz);
            _mqttServer = mqttServer;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var factory = new MqttFactory();
        //    var mqttClient = factory.CreateMqttClient();
        //    var mqttOptions = new MqttClientOptionsBuilder()
        //            .WithClientId("1fasdfioifiagsf")
        //            .WithKeepAlivePeriod(TimeSpan.FromHours(24))
        //            .WithKeepAliveSendInterval(TimeSpan.FromSeconds(5))
        //            .WithCleanSession()
        //            .WithWebSocketServer("192.168.31.101:5000/mqtt")
        //            .Build();
        //    await mqttClient.ConnectAsync(mqttOptions, new System.Threading.CancellationToken());

        //    await _mqttServer.PublishAsync("VueMqtt/publish1", "123123");

        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            return View(_biz);
        }

        [CustomOAuth(null, "/WxPay/OAuthCallback")]
        public async Task<IActionResult> WxPayIndex()
        {
            // 获取openId
            var openId = HttpContext.Session.GetString("OpenId");

            // 根据openId 查询用户
            var user = _authSrv.GetUserByExternalId(openId, 1);

            // 用户存在则获取信息，不存在则创建用户
            if (user == null)
            {
                var newUser = new BaseUser { UserName = openId, ExternalId = openId, ExternalType = 1 };
                var result = await _authSrv.CreateUser(newUser, openId);

                if (result.Succeeded)
                {
                    var role = await _authSrv.AddRolesAsync(openId, new string[] { "SysUser" });
                }
                user = newUser;

            }
            ViewData["credit"] = user.Credit;
            ViewData["biz"] = _biz;

            return View();
            // 前端菜单：直接支付，跳转js支付，余额支付跳转/Home/CreditPay，套餐支付：
        }

        [CustomOAuth(null, "/AliPay/OAuthCallback")]
        public async Task<IActionResult> AlipayIndex()
        {
            // 获取openId
            var openId = HttpContext.Session.GetString("OpenId");
            var type = HttpContext.Session.GetString("IdType");

            // 根据openId 查询用户
            var user = _authSrv.GetUserByExternalId(openId, 0);

            // 用户存在则获取信息，不存在则创建用户

            // 返回用户信息

            // 前端菜单：直接支付，跳转js支付，余额支付跳转/Home/CreditPay，套餐支付：

            return View();
        }

        [HttpPost("/Home/CreateOrder")]
        public async Task<IActionResult> CreateOrder(string id, PayMethod method)
        {
            try
            {
                var openId = HttpContext.Session.GetString("OpenId");
                //获取产品信息
                var product = _biz.Products.FirstOrDefault(z => z.Id == id);
                if (product == null)
                {
                    return Content("商品信息不存在，或非法进入！1002");
                }


                var o = new PaymentOrder()
                {
                    Content = product.Name,
                    Amount = product.Amount,
                    Quantity = product.Quantity,
                    PayMethod = product.CostType == "Cash" ? method : PayMethod.Credit,
                    OrderType = product.ProductType == "Credit" ? OrderType.AddCredit : OrderType.Buy,
                    UserId = openId,
                    OrderState = OrderState.WaitForPayment
                };
                var order = _paySrv.CreatePaymentOrder(o);

                if (o.OrderType == OrderType.Buy && method == PayMethod.Credit)
                {
                    var result = await CreditPay(order);
                    return Json(Url.Action("OrderResult", "Home", new { credit = result }));
                }
                else if (method == PayMethod.Wechat)
                {
                    return Json(Url.Action("JsApi", "WxPay", new { orderid = order.Id }));
                }
                else if (method == PayMethod.Alipay)
                {
                    return Json(Url.Action("JsApi", "Alipay", new { orderid = order.Id }));
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<decimal> CreditPay(PaymentOrder order)
        {
            var openId = HttpContext.Session.GetString("OpenId");
            var type = Int16.Parse(HttpContext.Session.GetString("IdType"));

            // 扣除余额
            var result = await _authSrv.UpdateCredit(openId, false, order.Amount);

            // 更新订单
            _paySrv.UpdatePaymentResult(order.Id, true);

            // 返回余额
            return result;
        }

        public IActionResult PaySuccess()
        {
            return View();
        }
    }
}