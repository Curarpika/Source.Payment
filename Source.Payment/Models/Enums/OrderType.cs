using System.ComponentModel;

namespace Source.Payment.Models.Enums
{
    public enum OrderType
    {
        [Description("产品购买")]
        AddCredit = 0,
        [Description("现金消费")]
        Buy = 1,
        Sell = 2
    }
}
