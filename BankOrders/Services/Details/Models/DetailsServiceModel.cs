namespace BankOrders.Services.Details
{
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;

    public class DetailsServiceModel
    {
        public int Id { get; set; }

        public int Branch { get; set; }

        public int CostCenter { get; set; }

        public int Project { get; set; }

        public string Reason { get; set; }

        public string Account { get; set; }

        public AccountType AccountType { get; set; }

        public decimal Sum { get; set; }

        public int CurrencyId { get; set; }

        public decimal SumBGN { get; set; }

        //public int AccountingNumber { get; set; }
    }
}
