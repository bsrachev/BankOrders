namespace BankOrders.Services.Currencies.Models
{
    public class CurrencyServiceModel
    {
        public int Id { get; init; }

        public string Code { get; init; }

        public decimal ExchangeRate { get; set; }
    }
}
