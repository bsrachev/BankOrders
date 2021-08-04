namespace BankOrders.Infrastructure
{
    using System.Linq;
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<BankOrdersDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            return app;
        }

        private static void SeedCategories(BankOrdersDbContext data)
        {
            if (data.ExchangeRates.Any())
            {
                return;
            }

            data.ExchangeRates.AddRange(new[]
            {
                new ExchangeRate { Currency = Currency.BGN, Rate = 1.00000m },
                new ExchangeRate { Currency = Currency.EUR, Rate = 1.95583m },
                new ExchangeRate { Currency = Currency.GBP, Rate = 2.29141m },
                new ExchangeRate { Currency = Currency.USD, Rate = 1.64563m },
            });

            data.SaveChanges();
        }
    }
}
