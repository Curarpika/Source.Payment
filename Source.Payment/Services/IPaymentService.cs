using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Source.Payment.Services
{
    public interface IPaymentService
    {
        (IQueryable<PaymentOrder> Orders, int Count)  GetPaymentOrders(string key, int index, int pageSize = 10, bool DateTimeDescending = true);
        IQueryable<PaymentOrder> GetPaidOrders();
        PaymentOrder GetPaymentOrderById(Guid id); 
        // 创建订单
        PaymentOrder CreatePaymentOrder(PaymentOrder order);

        // 回调更新支付状态
        PaymentOrder UpdatePaymentResult(Guid id, bool succeed);

        // 更新业务处理状态
        PaymentOrder ProcessPaymentOrder(Guid id);
    }
}