using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Newtonsoft.Json;
using Source.Database.Bases.Models;
using Source.Database.Bases.Helpers;
using Source.Database.Bases.Models.Enums;

public class ProductOrder : AuditEntity
{
    public string Snapshot{get;set;}
    public Guid? BuyerId {get;set;}
    public string BuyerIdentity {get;set;}
    public string ExcuteAddress {get;set;}
    public DateTime? ExcuteDateTime {get;set;}
    public int? Amount {get;set;}
    public int? CreditAmount {get;set;}
    public Guid? ProductOrderId {get;set;}
    public OrderState OrderState { get; set; }
}