namespace BankOrders.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using static BankOrders.Areas.Admin.AdminConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;

            MigrateDatabase(serviceProvider);

            SeedCurrencies(serviceProvider);
            SeedAdministrator(serviceProvider);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<BankOrdersDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCurrencies(IServiceProvider services)
        {
            var data = services.GetRequiredService<BankOrdersDbContext>();

            if (data.Currencies.Any())
            {
                return;
            }

            data.Currencies.AddRange(new[]
            {
                new Currency { Code = "BGN", ExchangeRate = 1.00000m },
                new Currency { Code = "EUR", ExchangeRate = 1.95583m },
            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    var user = new User
                    {
                        Email = "admin@bankorders.com",
                        UserName = "BO001",
                        FullName = "Admin",
                        EmployeeNumber = "BO001"
                    };

                    await userManager.CreateAsync(user, "theadmin");

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
