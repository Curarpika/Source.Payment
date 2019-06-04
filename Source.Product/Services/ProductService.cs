using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Source.Database.Bases.Models;
using Source.Product.Models;

namespace Source.Product.Services
{
    public class ProductService : IProductService
    {
        private readonly GenericRepository<ProductOrder> _orderRepo;
        public ProductService(GenericRepository<ProductOrder> orderRepo)
        {            
            _orderRepo = orderRepo;
        }

        public ProductOrder CreateProductOrder(ProductOrder order)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProductOrder> GetPaidOrders()
        {
            throw new NotImplementedException();
        }

        public ProductOrder GetProductOrderById(Guid id)
        {
            throw new NotImplementedException();
        }

        public (IQueryable<ProductOrder> Orders, int Count) GetProductOrders(string key, int index, int pageSize = 10, bool DateTimeDescending = true)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProductOrder> GetProductOrders()
        {
            throw new NotImplementedException();
        }

        public ProductOrder ProcessProductOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        public ProductOrder UpdateProductResult(Guid id, bool succeed)
        {
            throw new NotImplementedException();
        }
    }
}