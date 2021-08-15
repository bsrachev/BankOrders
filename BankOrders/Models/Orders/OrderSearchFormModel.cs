namespace BankOrders.Models.Orders
{
    using BankOrders.Data.Models.Enums;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class OrderSearchFormModel
    {
        public string RefNumber { get; set; }

        [DataType(DataType.Date)]
        public string AccountingDateFrom { get; set; }

        [DataType(DataType.Date)]
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
