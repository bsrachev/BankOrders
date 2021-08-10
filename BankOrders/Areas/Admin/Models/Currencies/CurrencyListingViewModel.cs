namespace BankOrders.Areas.Admin.Models.Currencies
{
    public class CurrencyListingViewModel
    {
        public int Id { get; init; }

        public string Code { get; init; }

        public decimal ExchangeRate { get; set; }
    }
}
