using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Newtonsoft.Json;
using Source.Database.Bases.Models;
using Source.Database.Bases.Helpers;
using Source.Product.Models.Enums;
using System.Collections.Generic;

namespace Source.Product.Models
{
    public class ProductOrder : AuditEntity
    {
        public OrderType? OrderType { get; set; }
        public string Content { get; set; }
        public string Snapshot{get;set;}
        public Guid? BuyerId {get;set;}
        public string BuyerIdentity {get;set;} 
        
        public int? Quantity { get; set; }    
        public decimal? Amount { get; set; }
        public decimal? CreditAmount {get;set;}
        public Guid? PaymentOrderId {get;set;}
        public DateTime? ExecuteDateTime {get;set;}
        public OrderState OrderState { get; set; }
        public string Contact {get;set;}
        public string ExcuteAddress {get;set;}
        public DateTime? ExecutedDatedTime {get;set;}

        [NotMapped]
        public IList<CartInfo> Carts{ get; set; }
    }

    public class CartInfo
    {
        
        public IList<CartItem> CartItems {get;set;}
        public decimal? Amount {get;set;}
    }

    public class CartItem
    {
        public Guid ProductId{get;set;}
        public Product Product {get;set;}
        public int Quantity {get;set;}
    }
    


}
