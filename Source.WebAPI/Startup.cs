using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPay;
using Source.Auth.Models;
using Source.Auth.Services;
using Source.Database.Bases.Models;
using Source.Database.Bases.Interfaces;
using Source.Payment.Models;
using Source.Payment.Services;
using Source.Database.Bases.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Source.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BaseAuthDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Source.WebAPI")));
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Source.WebAPI")));

            var mqttServerOptions = new MqttServerOptionsBuilder()
                .WithoutDefaultEndpoint()
                .Build();
            //this adds a hosted mqtt server to the services
            services.AddHostedMqttServer(mqttServerOptions);
            //this adds tcp server support based on Microsoft.AspNetCore.Connections.Abstractions
            services.AddMqttConnectionHandler();
            //this adds websocket support
            services.AddMqttWebSocketServerAdapter();

            //     // Identity
            services.AddIdentity<BaseUser, BaseRole>()
           .AddEntityFrameworkStores<BaseAuthDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = false;
            });

            // services.Configure<CookiePolicyOptions>(options =>
            // {
            //     // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //     options.CheckConsentNeeded = context => true;
            //     options.MinimumSameSitePolicy = SameSiteMode.None;
            // });

            // Cors
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Source.WebAPI", Version = "v1" });

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Source.WebAPI.xml");
                c.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();
            services.AddSession();//使用Session

            // services.AddSingleton<IHostedService>(s => s.GetService<MqttHostedServer>()); 
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, EntityFrameworkUnitOfWork>();                

            services.AddTransient<IDbContext, PaymentDbContext>();
            services.AddTransient<IDbContext, ProductDbContext>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IPaymentService, PaymentService>();


            services.AddSenparcGlobalServices(Configuration)//Senparc.CO2NET 全局注册
                .AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //引入EnableRequestRewind中间件
            app.UseEnableRequestRewind();
            app.UseSession();

            app.UseMqttEndpoint();

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", $"API V1 [{env.EnvironmentName}]");
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value)
                                                        //关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
                                                        .UseSenparcGlobal();

            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value)
                .RegisterTenpayV3(senparcWeixinSetting.Value, "【盛派网络小助手】公众号");
            //AccessTokenContainer.Register(appId, appSecret, name);//命名空间：Senparc.Weixin.MP.Containers

        }
    }
}
