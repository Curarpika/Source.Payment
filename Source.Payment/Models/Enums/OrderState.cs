using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Payment.Models.Enums
{
    public enum OrderState
    {
        [Description("等待支付")]
        WaitForPayment = 0,
        [Description("已支付")]
        Paid = 1,
        [Description("支付失败")]
        PayFailed = 2
    }
}
