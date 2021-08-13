namespace BankOrders.Services.Currencies
{
    using BankOrders.Services.Currencies.Models;
    using System.Collections.Generic;

    public interface ICurrencyService
    {
        IEnumerable<CurrencyServiceModel> GetCurrencies();

        int Add(string code);

        void Delete(int id);

        CurrencyServiceModel GetCurrencyInfo(int currencyId);
    }
}
