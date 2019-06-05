using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Newtonsoft.Json;
using Source.Database.Bases.Models;
using Source.Database.Bases.Helpers;

namespace Source.Product.Models
{
    public class Product : AuditEntity
    {
        public int? ProductType {get;set;}
        public string Name{get;set;}
        public string Unit {get;set;}
        public string Spec {get;set;}
        public int? Price {get;set;}
        public int? Credit {get;set;}
        public double Discount { get;set; } = 1.0;
        public int? InStock {get;set;}
        public Guid? SupplierId { get; set; }
        public byte[] Cover {get;set;}
        public string Images {get;set;}
        public string SupplierIdentity { get; set; }
        public bool IsAvailable {get;set;} 
    }
}