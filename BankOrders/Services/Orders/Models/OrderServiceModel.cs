namespace BankOrders.Services.Orders
{
    using BankOrders.Data.Models.Enums;

    using System;

    public class OrderServiceModel
    {
        public int Id { get; set; }

        public int RefNumber { get; set; }

        public DateTime AccountingDate { get; set; }

        public OrderSystem System { get; set; }

        public string UserCreateId { get; set; }

        public string UserApproveId { get; set; }

        public string UserPostingId { get; set; }

        public string UserApprovePostingId { get; set; }

        public OrderStatus Status { get; set; }

        public int PostingNumber { get; set; }
    }
}
