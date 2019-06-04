using System;
using System.IO;

namespace Source.Database.Bases.Models
{
    public class Business
    {
        public Price[] Products { get; set; }
    }

    public class Price
    {
        public string Id {get;set;}
        public string Name {get;set;}
        public decimal Amount { get; set; }
        public string CostType {get;set;}

        public string ProductType {get;set;}
        public int Quantity { get; set;}
        public string Describe {get;set;}
    }
}