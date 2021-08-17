namespace BankOrders.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BankOrders.Data.Models.Enums;

    public class Order : BaseDocument
    {
        public Order()
        {
            this.Details = new HashSet<Detail>();
            this.Status = 0;
        }

        [Required]
        public DateTime AccountingDate { get; set; }

        public string UserApproveId { get; set; }

        public User UserApprove { get; set; }

        public string UserPostingId { get; set; }

        public User UserPosting { get; set; }

        public string UserApprovePostingId { get; set; }

        public User UserApprovePosting { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public int PostingNumber { get; set; }
    }
}
