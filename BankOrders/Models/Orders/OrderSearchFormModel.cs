namespace BankOrders.Models.Orders
{
    using BankOrders.Data.Models.Enums;

    using System;

    public class OrderSearchFormModel // formerly OrderQueryServiceModel
    {
        public string RefNumber { get; set; }

        public string AccountingDateFrom { get; set; }

        public string AccountingDateTo { get; set; }

        public string System { get; set; }

        public string UserCreateId { get; set; }

        public string UserApproveId { get; set; }

        public string UserPostingId { get; set; }

        public string UserApprovePostingId { get; set; }

        public string Status { get; set; }

        public string IsLocked { get; set; }
    }
}
