using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Source.WebAPI.Interfaces;
using Source.Payment.Models.Base.Converters;

namespace Source.WebAPI.Bases
{
    public abstract class AuditEntity: IAuditEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        public string CreatedBy { get; set; }
        [JsonConverter(typeof(ShortDateConverter))]
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        [JsonConverter(typeof(ShortDateConverter))]
        public DateTime? UpdatedTime { get; set; }
    }
}
