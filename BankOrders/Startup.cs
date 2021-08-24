namespace BankOrders
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Infrastructure;
    using BankOrders.Services.Orders;
    using BankOrders.Services.Details;
    using BankOrders.Services.Templates;
    using BankOrders.Services.Users;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using BankOrders.Services.Currencies;
    using BankOrders.Services.Email;
    using System.Text.Json;
    using System.IO;

    public class Startup
    {
        public Startup(IConfiguration configuration)
            => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<BankOrdersDbContext>(options => options
                    .UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BankOrdersDbContext>();

            //services
            //    .AddControllersWithViews();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDetailService, DetailService>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IEmailService, EmailService>();
            
            var mailSettings = File.ReadAllText("Properties/mailSettings.json");
            var options = JsonSerializer.Deserialize<EmailSenderOptions>(mailSettings);

            services.Configure<EmailSenderOptions>(o =>
            {
                o.Host_Address = options.Host_Address;
                o.Host_Port = options.Host_Port;
                o.Host_Username = options.Host_Username;
                o.Host_Password = options.Host_Password;
                o.Sender_EMail = options.Sender_EMail;
                o.Sender_Name = options.Sender_Name;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultAreaRoute();
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                    /*endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Login}/{id?}");*/
                });
        }
    }
}
