namespace BankOrders.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BankOrders.Data.Models.Enums;

    public class Detail
    {
        [Key]
        public int Id { get; set; }

        //public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int OrderOrTemplateRefNum { get; set; }

        public int Branch { get; set; }

        public int CostCenter { get; set; }

        public int Project { get; set; }

        [Required]
        [MaxLength(200)]
        public string Reason { get; set; }

        [Required]
        [MaxLength(200)]
        public string Account { get; set; }

        [Required]
        [MaxLength(2)]
        public AccountType AccountType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Sum { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public Currency Currency { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal SumBGN { get; set; }

        //public int AccountingNumber { get; set; }
    }
}
