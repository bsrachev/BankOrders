namespace BankOrders.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BankOrders.Data.Models.Enums;
    using PaymentOrders.Data.Models;

    public class Order : BaseDocument
    {
        public Order()
        {
            //this.Id = Guid.NewGuid().ToString();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Status = 0;
        }

        //[Key]
        //[Required]
        //public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(20)]
        public int RefNumber { get; set; }

        [Required]
        public DateTime? AccountingDate { get; set; }

        public string UserApprove { get; set; }

        public string UserAccountant { get; set; }

        public string UserApproveAccounting { get; set; }

        [Required]
        public OrderStatus Status { get; set; }
    }
}
