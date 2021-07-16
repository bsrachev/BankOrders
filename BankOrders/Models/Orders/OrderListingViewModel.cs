namespace BankOrders.Models.Orders
{
    using System;

    using BankOrders.Data.Models.Enums;

    public class OrderListingViewModel
    {
        public string Id { get; set; }

        public int RefNumber { get; set; }

        public DateTime? AccountingDate { get; set; }

        public OrderSystem System { get; set; }

        public string UserCreate { get; set; }

        public string UserApprove { get; set; }

        public string UserAccountant { get; set; }

        public string UserApproveAccounting { get; set; }

        public OrderStatus Status { get; set; }
    }
}
