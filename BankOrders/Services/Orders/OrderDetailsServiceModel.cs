namespace BankOrders.Services.Orders
{
    using BankOrders.Data.Models.Enums;

    public class OrderDetailsServiceModel : OrderServiceModel
    {
        public int Branch { get; set; }

        public int CostCenter { get; set; }

        public int Project { get; set; }

        public string Reason { get; set; }

        public string Account { get; set; }

        public AccountType AccountType { get; set; }

        public decimal? Sum { get; set; }

        public Currency Currency { get; set; }

        public decimal? SumBGN { get; set; }

        public int AccountingNumber { get; set; }
    }
}
