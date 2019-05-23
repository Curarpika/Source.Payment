using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Payment.Models.Enums
{
    public enum OrderState
    {
        WaitForPayment = 0,
        Paid = 1,
        PayFailed = 2,
        Processed = 3
    }
}
