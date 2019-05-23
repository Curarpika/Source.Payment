using System;
using System.IO;

namespace Source.Payment
{
    public class Business
    {
        public Price Price { get; set; }
        public Price[] Bundles { get; set; }
    }

    public class Price
    {
        public decimal Amount { get; set; }
        public int Quantity { get; set;}
    }
}