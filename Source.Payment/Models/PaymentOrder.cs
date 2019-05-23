using System;
using System.IO;
using Newtonsoft.Json;
using Source.Payment.Models.Base;
using Source.Payment.Models.Base.Converters;
using Source.Payment.Models.Enums;

namespace Source.Payment
{
    public class PaymentOrder : BaseEntity
    {
        public decimal Amount { get; set; }
        public OrderType OrderType { get; set;}
        public PayMethod PayMethod { get; set;}
        public String PaymentOrderId{ get; set; }
        [JsonConverter(typeof(ShortDateConverter))]
        public DateTime? PaidTime { get; set; }
        public bool Processed {get;set;}
    }
}