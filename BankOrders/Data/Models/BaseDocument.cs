namespace BankOrders.Data.Models
{
    using BankOrders.Data.Models.Enums;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseDocument
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(20)]
        public int RefNumber { get; init; }

        [Required]
        public OrderSystem System { get; init; }

        [Required]
        public string UserCreateId { get; init; }

        public User UserCreate { get; init; }

        public ICollection<Detail> Details { get; set; }
    }
}
