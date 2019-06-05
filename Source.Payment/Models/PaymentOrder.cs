using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Newtonsoft.Json;
using Source.Payment.Models;
using Source.Payment.Helpers;
using Source.Payment.Interfaces;
using Source.Payment.Models.Enums;

namespace Source.Payment.Models
{
    public class PaymentOrder : AuditEntity
    {
        // public string Content { get; set; }
        public string UserId { get; set; }
        public Guid? OrderId { get; set; }
        public decimal Amount { get; set; }
        // public int Quantity { get; set; }
        // public OrderType OrderType { get; set; }
        public PayMethod PayMethod { get; set; }
        public OrderState OrderState { get; set; }
        // 回调订单ID
        public String PaymentOrderId { get; set; }

        [JsonConverter(typeof(ShortDateConverter))]
        public DateTime? PaidTime { get; set; }
        [NotMapped]
        public string Code
        {
            get
            {
                return GuidEncoder.Encode(Id).Substring(0, 4).ToUpper();
            }
        }
    }

}