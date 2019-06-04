using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Source.Product.Services
{
    public interface IProductService
    {
        (IQueryable<ProductOrder> Orders, int Count)  GetProductOrders(string key, int index, int pageSize = 10, bool DateTimeDescending = true);
        IQueryable<ProductOrder> GetProductOrders();
        IQueryable<ProductOrder> GetPaidOrders();
        ProductOrder GetProductOrderById(Guid id); 
        // 创建订单
        ProductOrder CreateProductOrder(ProductOrder order);

        // 回调更新支付状态
        ProductOrder UpdateProductResult(Guid id, bool succeed);

        // 更新业务处理状态
        ProductOrder ProcessProductOrder(Guid id);
    }
}