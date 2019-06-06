using Microsoft.AspNetCore.Identity;
using Source.Product.Models;
using Source.Product.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Source.Product.Services
{
    public interface IProductOrderService
    {
        (IQueryable<ProductOrder> Orders, int Count) GetProductOrders(string key, int? orderType, Guid? buyerId, int? orderState, int pageIndex, int pageSize, bool? dateTimeDescending);
        IQueryable<ProductOrder> GetProductOrders();
        IQueryable<ProductOrder> GetPaidOrders();
        ProductOrder GetProductOrderByPaymentId(Guid paymentId);
        ProductOrder GetProductOrderById(Guid id); 
        // 创建订单
        ProductOrder CreateProductOrder(ProductOrder order);

        // 回调更新订单状态
        ProductOrder UpdateProductOrder(Guid id, OrderState state, Guid? payId = null);
    }
}