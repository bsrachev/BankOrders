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

        public string UserCreate { get; set; }

        public string UserApprove { get; set; }

        public string UserPosting { get; set; }

        public string UserApprovePosting { get; set; }

        public string Status { get; set; }

        public string PostingNumber { get; set; }
    }
}
