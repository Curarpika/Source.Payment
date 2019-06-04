using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore;
using Source.Auth.Models;
using Source.Payment.Models;
using Source.Product.Models;

namespace Source.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var identityContext = scope.ServiceProvider.GetService<BaseAuthDbContext>();
                identityContext.Database.Migrate();
                identityContext.EnsureSeedData();

                var paymentContext = scope.ServiceProvider.GetService<PaymentDbContext>();
                paymentContext.Database.Migrate();

                var productContext = scope.ServiceProvider.GetService<ProductDbContext>();
                productContext.Database.Migrate();

            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("business.json", optional: false, reloadOnChange: false);
                })
                .UseKestrel(o => {
                    o.ListenAnyIP(5050, l => l.UseMqtt()); // mqtt pipeline
                    o.ListenAnyIP(80); // default http pipeline
                })
                .UseStartup<Startup>();
    }
}
