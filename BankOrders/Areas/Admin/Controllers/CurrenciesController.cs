namespace BankOrders.Areas.Admin.Controllers
{
    using BankOrders.Areas.Admin.Models.Currencies;
    using BankOrders.Data;
    using BankOrders.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class CurrenciesController : AdminController
    {
        private readonly BankOrdersDbContext data;

        public CurrenciesController(BankOrdersDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var currencies = this.data
                .Currencies
                .Select(c => new CurrencyListingViewModel
                {
                    Id = c.Id,
                    Code = c.Code,
                    ExchangeRate = c.ExchangeRate
                })
                .ToList();

            return this.View(currencies);
        }
    }
}
