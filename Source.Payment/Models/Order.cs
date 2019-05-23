using System;
using System.IO;

namespace Source.Payment
{
    public class Order
    {
        public Price Price { get; set; }
        public Price[] Bundles { get; set; }
    }
}