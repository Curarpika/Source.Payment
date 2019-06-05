using Microsoft.AspNetCore.Identity;
using Source.Database.Bases.Interfaces;
using Source.Database.Bases.Models;
using Source.Product.Models;
using Source.Product.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Source.Product.Services
{
    public class ProductService : IProductService, IProductOrderService
    {
        private readonly IRepository<ProductOrder> _orderRepo;
        private readonly IRepository<Models.Product> _prdRepo;
        private readonly IUnitOfWork _uow;
        public ProductService(IRepository<ProductOrder> orderRepo,
        IRepository<Models.Product> prdRepo,
        IUnitOfWork uow)
        {            
            _orderRepo = orderRepo;
            _prdRepo = prdRepo;
            _uow = uow;
        }

        #region Product 产品

        public Models.Product AddProduct(Models.Product prd)
        {
            _prdRepo.Add(prd);
            _prdRepo.Save();
            return prd;
        }

        public Models.Product RemoveProduct(Guid prdId)
        {
            var prd = _prdRepo.Find(x=>x.Id == prdId).FirstOrDefault();
            _prdRepo.Delete(prd);
            _prdRepo.Save();
            return prd;
        }

        public Models.Product GetProductById(Guid prdId)
        {
            var prd = _prdRepo.Find(x=>x.Id == prdId).FirstOrDefault();
            return prd;
        }

        public IQueryable<Models.Product> GetProductByIds(Guid[] prdIds)
        {
            var prds = _prdRepo.Find(x=>prdIds.Contains(x.Id));
            return prds;
        }

        (IQueryable<Models.Product> Products, int Count) IProductService.GetProducts(string key, int? productType, bool? isAvailable, Guid? supplierId, int pageIndex, int pageSize, bool? dateTimeDescending)        {
            try
            {
                var prds = _prdRepo.All();

                if (!string.IsNullOrEmpty(key))
                {
                    prds = prds.Where(q => q.Name.Contains(key));
                }

                if (productType != null)
                {
                    prds = prds.Where(q => q.ProductType == productType);
                }

                if (isAvailable != null)
                {
                    prds = prds.Where(q => q.IsAvailable == isAvailable);
                }

                if (supplierId != null)
                {
                    prds = prds.Where(q => q.SupplierId == supplierId);
                }

                var total = prds.Count();
                if (dateTimeDescending != null)
                {
                    prds = (bool)dateTimeDescending ? prds.OrderByDescending(e => e.CreatedTime): prds.OrderBy(e => e.CreatedTime);
                }
                if (pageSize > 0 && pageIndex > 0)
                {
                    prds = prds.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                return (prds, total);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ProductOrder 产品订单
        public ProductOrder CreateProductOrder(ProductOrder order)
        {

            try
            {
                _orderRepo.Add(order);
                _orderRepo.Save();
                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<ProductOrder> GetPaidOrders()
        {
            try
            {
                var paidOrders = _orderRepo.Find(x => x.OrderState == OrderState.Excuting);
                return paidOrders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductOrder GetProductOrderById(Guid id)
        {
            try
            {
                var order = _orderRepo.Find(x => x.Id == id).FirstOrDefault();
                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ProductOrder GetProductOrderByPaymentId(Guid paymentId)
        {
            try
            {
                var order = _orderRepo.Find(x => x.PaymentOrderId == paymentId).FirstOrDefault();
                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<ProductOrder> GetProductOrders()
        {
            try
            {
                return _orderRepo.All();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductOrder UpdateProductOrder(Guid id, OrderState state)
        {
            try
            {
                var order = _orderRepo.Find(x => x.Id == id).FirstOrDefault();
                if (order == null)
                {
                    return null;
                }
                order.OrderState = state;
                _orderRepo.Save();
                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        (IQueryable<ProductOrder> Orders, int Count) IProductOrderService.GetProductOrders(string key, int? orderType, Guid? buyerId, int? orderState, int pageIndex, int pageSize, bool? dateTimeDescending)
        {
            try
            {
                var orders = _orderRepo.All();

                if (!string.IsNullOrEmpty(key))
                {
                    orders = orders.Where(q => q.Content.Contains(key));
                }

                if (orderType != null)
                {
                    orders = orders.Where(q => q.OrderType == (OrderType)orderType);
                }

                if (buyerId != null)
                {
                    orders = orders.Where(q => q.BuyerId == buyerId);
                }

                if (orderState != null)
                {
                    orders = orders.Where(q => q.OrderState == (OrderState)orderState);
                }

                var total = orders.Count();
                if (dateTimeDescending != null)
                {
                    orders = (bool)dateTimeDescending ? orders.OrderByDescending(e => e.CreatedTime): orders.OrderBy(e => e.CreatedTime);
                }

                if (pageSize > 0 && pageIndex > 0)
                {
                    orders = orders.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                return (orders, total);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion
    }
}