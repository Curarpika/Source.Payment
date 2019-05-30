using Microsoft.AspNetCore.Identity;
using Source.Payment.Interfaces;
using Source.Payment.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Source.Payment.Services
{
    public class PaymentService : IPaymentService
    {
         private readonly IRepository<PaymentOrder> _orderRepo;
        public PaymentService(IRepository<PaymentOrder> orderRepo)
        {            
            _orderRepo = orderRepo;
        }
        public PaymentOrder CreatePaymentOrder(PaymentOrder order)
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

        public IQueryable<PaymentOrder> GetPaidOrders()
        {
            try
            {
                var paidOrders = _orderRepo.Find(x => x.OrderState == OrderState.Paid);
                return paidOrders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PaymentOrder GetPaymentOrderById(Guid id)
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


        public PaymentOrder ProcessPaymentOrder(Guid id)
        {
            try
            {
                var order = _orderRepo.Find(x => x.Id == id).FirstOrDefault();
                if(order == null)
                {
                    return null;
                }
                order.OrderState = OrderState.Processed;
                _orderRepo.Save();
                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PaymentOrder UpdatePaymentResult(Guid id, bool succeed)
        {
            try
            {
                var order = _orderRepo.Find(x => x.Id == id).FirstOrDefault();
                if(order == null)
                {
                    return null;
                }
                order.OrderState = succeed? OrderState.Paid : OrderState.PayFailed;
                _orderRepo.Save();
                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

       (IQueryable<PaymentOrder> Orders, int Count)  IPaymentService.GetPaymentOrders(string key, int index, int pageSize, bool DateTimeDescending)
        {
            try
            {
                var paidOrders = _orderRepo.Find(x => x.Content.Contains(key));
                var total = paidOrders.Count();
                if (pageSize > 0 && index > 0)
                {
                    paidOrders = paidOrders.OrderByDescending(e => e.CreatedTime).Skip((index - 1) * pageSize).Take(pageSize);
                }
                return (paidOrders,total);                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}