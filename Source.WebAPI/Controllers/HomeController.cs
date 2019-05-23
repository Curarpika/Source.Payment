using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using Source.Payment;

namespace Source.WebAPI
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IMqttServer _mqttServer;
        public HomeController(IConfiguration config, IMqttServer mqttServer)
        {
            _config = config;
            _mqttServer = mqttServer;
        }

        public async Task<IActionResult> Index()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var mqttOptions = new MqttClientOptionsBuilder()
                    .WithClientId("1fasdfioifiagsf")
                    .WithKeepAlivePeriod(TimeSpan.FromHours(24))
                    .WithKeepAliveSendInterval(TimeSpan.FromSeconds(5))
                    .WithCleanSession()
                    .WithWebSocketServer("192.168.31.101:5000/mqtt")
                    .Build();
            await mqttClient.ConnectAsync(mqttOptions, new System.Threading.CancellationToken());

            await _mqttServer.PublishAsync("VueMqtt/publish1","123123");

            Business biz = new Business();
            _config.GetSection("Business").Bind(biz);
            return new JsonResult(biz);
        }
    }
}