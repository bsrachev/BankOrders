namespace BankOrders.Models.Orders
{
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.OrderDetails;
    using System;
    using System.Collections.Generic;

    public class OrderDetailListingViewModel
    {
        public int Id { get; set; }

        public int? EditDetailId { get; set; }

        public int RefNumber { get; set; }

        public DateTime? AccountingDate { get; set; }

        public OrderSystem System { get; set; }

        public string UserCreate { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderDetailFormModel> OrderDetails { get; set; }

        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
    }
}
