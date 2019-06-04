using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Product.Interfaces
{
    public interface IAuditEntity
    {
        string CreatedBy { get; set; }
        DateTime CreatedTime { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedTime { get; set; } 
    }
}
