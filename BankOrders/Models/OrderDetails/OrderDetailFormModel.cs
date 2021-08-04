namespace BankOrders.Models.OrderDetails
{
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;

    using System.Collections.Generic;

    public class OrderDetailFormModel
    {
        public int OrderDetailId { get; init; }

        public int Branch { get; init; }

        public int CostCenter { get; init; }

        public int Project { get; init; }

        public string Reason { get; init; }

        public string Account { get; init; }

        public AccountType AccountType { get; init; }

        public decimal Sum { get; init; }

        public Currency Currency { get; init; }

        public decimal SumBGN { get; init; }

        public int AccountingNumber { get; init; }

        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }

        public OrderSystem OrderSystem { get; set; }
    }
}
