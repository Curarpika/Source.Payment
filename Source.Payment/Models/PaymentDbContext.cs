using Microsoft.EntityFrameworkCore;  
  
namespace Source.Payment.Models
{  
    public class PaymentDbContext : DbContext  
    {  
        public PaymentDbContext(DbContextOptions options) : base(options)  
        {  
        }  
  
        DbSet<PaymentOrder> PaymentOrders { get; set; }  
    }  
}  