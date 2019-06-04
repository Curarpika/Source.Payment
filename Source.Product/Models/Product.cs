using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Newtonsoft.Json;
using Source.Database.Bases.Models;
using Source.Database.Bases.Helpers;

public class Product : AuditEntity
{
    public string Name{get;set;}
    public string Unit {get;set;}
    public string Spec {get;set;}
    public int? Price {get;set;}
    public int? Credit {get;set;}
    public int? InStock {get;set;}
    public Guid? SupplierId { get; set; }
    public string SupplierIdentity { get; set; }
}