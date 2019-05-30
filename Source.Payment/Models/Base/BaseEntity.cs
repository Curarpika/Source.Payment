using Newtonsoft.Json;
using Source.Payment.Models.Base.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Payment.Models.Base
{
    public class BaseEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        [JsonConverter(typeof(ShortDateConverter))]
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        [JsonConverter(typeof(ShortDateConverter))]
        public DateTime? UpdatedTime { get; set; }
    }
}
