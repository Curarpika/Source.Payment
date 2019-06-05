using System.ComponentModel;

namespace Source.Product.Models.Enums
{
    public enum OrderType
    {
        [Description("积分充值")]
        AddCredit = 0,
        [Description("线上消费")]
        BuyOnline = 1,
        [Description("线下消费")]
        BuyInStore = 2,
        Sell = 3
    }
}
