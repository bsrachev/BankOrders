namespace BankOrders.Areas.Admin.Controllers
{
    using BankOrders.Areas.Admin.Models.Currencies;
    using BankOrders.Data;
    using BankOrders.Infrastructure;
    using BankOrders.Services.Currencies;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.SuccessMessages;
    using static WebConstants;

    public class CurrenciesController : AdminController
    {
        private readonly ICurrencyService currencyService;
        private readonly BankOrdersDbContext data;

        public CurrenciesController(ICurrencyService currencyService, BankOrdersDbContext data)
        {
            this.currencyService = currencyService;
            this.data = data;
        }

        public IActionResult Index()
        {
            var currencies = this.currencyService.GetCurrencies();

            return this.View(currencies);
        }

        [HttpPost]
        public IActionResult Index(CurrencyFormModel currencyModel)
        {
            if (this.currencyService.GetCurrencies().Any(c => c.Code == currencyModel.Code))
            {
                TempData[GlobalErrorKey] = CurrencyCodeAlreadyExists;
            }
            else if (this.currencyService.Add(currencyModel.Code) == 0)
            {
                TempData[GlobalErrorKey] = InvalidCurrencyCode;
            }
            else
            {
                TempData[GlobalSuccessKey] = SuccessfullyAddedCurrency;
            }

            var currencies = this.currencyService.GetCurrencies();
                
            return this.View(currencies);
        }

        public IActionResult Delete(int id)
        {
            if (id == 1 || id == 2)
            {
                TempData[GlobalErrorKey] = CannotDeleteEURorBGN;

                return this.RedirectToAction(nameof(Index));
            }

            this.currencyService.Delete(id);

            TempData[GlobalSuccessKey] = SuccessfullyDeletedCurrency;

            return this.RedirectToAction(nameof(Index));
        }
    }
}
