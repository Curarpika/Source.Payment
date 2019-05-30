using System;
using System.IO;
using Newtonsoft.Json;
using Source.Payment.Bases;
using Source.Payment.Models.Base;
using Source.Payment.Models.Base.Converters;
using Source.Payment.Models.Enums;

namespace Source.Payment
{
    public class PaymentOrder : AuditEntity
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public OrderType OrderType { get; set;}
        public PayMethod PayMethod { get; set;}
        public OrderState OrderState { get; set;}
        // 回调订单ID
        public String PaymentOrderId{ get; set; }
        [JsonConverter(typeof(ShortDateConverter))]
        public DateTime? PaidTime { get; set; }
    }
}