namespace BankOrders.Services.Orders
{
    using BankOrders.Data.Models.Enums;

    using System;

    public class OrderServiceModel
    {
        public int Id { get; set; }

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
