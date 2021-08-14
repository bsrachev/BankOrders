namespace BankOrders.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    //using System.Data.Entity;

    using BankOrders.Data.Models.Enums;

    public class Order : BaseDocument
    {
        public Order()
        {
            //this.Id = Guid.NewGuid().ToString();
            this.Details = new HashSet<Detail>();
            this.Status = 0;
        }

        //[Key]
        //[Required]
        //public string Id { get; set; } = Guid.NewGuid().ToString();

        //[Required]
        //[MaxLength(20)]
        //public int RefNumber { get; set; }

        [Required]
        public DateTime? AccountingDate { get; set; }

        public string UserApproveId { get; set; }

        public string UserPostingId { get; set; }

        public string UserApprovePostingId { get; set; }

        [Required]
        public OrderStatus Status { get; set; }
    }
}
