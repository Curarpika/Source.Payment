using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Product.Models.Enums
{
    public enum PayMethod
    {
        [Description("支付宝")]
        Alipay = 0,
        [Description("微信")]
        Wechat = 1,
        [Description("余额")]
        Credit = 2
    }
}
