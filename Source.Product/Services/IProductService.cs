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
    public interface IProductService
    {
        Models.Product AddProduct(Models.Product prd);

        Models.Product RemoveProduct(Guid prdId);
        Models.Product GetProductById(Guid prdId);
        IQueryable<Models.Product> GetProductByIds(Guid[] prdIds);

        (IQueryable<Models.Product> Products, int Count) GetProducts(string key, int? productType, bool? isAvailable, Guid? supplierId, int pageIndex, int pageSize, bool? dateTimeDescending);
    }
}