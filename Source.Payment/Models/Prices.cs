using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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