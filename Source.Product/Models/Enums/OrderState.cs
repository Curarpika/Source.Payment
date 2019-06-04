using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Database.Bases.Models.Enums
{
    public enum OrderState
    {
        [Description("已下单")]
        Ordered = 0,
        [Description("执行中")]
        Excuting = 1,
        [Description("订单完成")]
        Excuted = 2,
    }
}
