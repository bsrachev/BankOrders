namespace BankOrders.Data.Models
{
    using BankOrders.Data.Models.Enums;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public int RefNumber { get; set; }

        [Required]
        public OrderSystem System { get; set; }

        [Required]
        public string UserCreate { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
