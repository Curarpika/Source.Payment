using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Payment.Models.Base.Converters
{
    public class ShortDateConverter : IsoDateTimeConverter
    {
        public ShortDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
