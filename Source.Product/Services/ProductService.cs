using Microsoft.AspNetCore.Identity;
using Source.Database.Bases.Interfaces;
using Source.Database.Bases.Models;
using Source.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Source.Product.Services
{
    public class ProductService : IProductService
    {
        private readonly GenericRepository<ProductOrder> _orderRepo;
        private readonly GenericRepository<Models.Product> _prdRepo;
        private readonly IUnitOfWork _uow;
        public ProductService(GenericRepository<ProductOrder> orderRepo,
        GenericRepository<Models.Product> prdRepo,
        IUnitOfWork uow)
        {            
            _orderRepo = orderRepo;
            _prdRepo = prdRepo;
            _uow = uow;
        }

        public ProductOrder CreateProductOrder(ProductOrder order)
        {
            _orderRepo.Add(order);
            _uow.Save();
            return order;
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