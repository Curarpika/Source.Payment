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
    public class ProductController : Controller
    {
        public IActionResult PickProducts()
        {
            // ---------自助餐-----------
            // 展示菜单

            // 选择份数

            // 选菜 =》分别保存菜品对象[{},{}]

            // 查询配置价格 x 份数 = 总价

            // 完成方式选择：堂食/（外卖、地址）

            // 提交 ProductOrder 替换总价
            
            // 选择支付方式 创建支付订单

            // 核销码/ 外送核销
            

            // ---------课程----------
            // 展示课程（美术课/书法，价格 积分/金额）

            // 选择课时数

            // 保存课程快照

            // 完成方式选择：上课时间

            // 提交 ProductOrder 计算总价
            
            // 选择支付方式 创建支付订单

            // 核销/签到

            // ---------驾考----------
            // 展示课程（驾考课程，价格 积分/金额）

            // 计算人数分摊金额

            // 保存课程快照

            // 完成方式选择：地址

            // 提交 ProductOrder 计算总价
            
            // 选择支付方式 创建支付订单

            // 签到/完成

             
        }
    }
}