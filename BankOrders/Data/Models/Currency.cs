namespace BankOrders.Data.Models
{
    using BankOrders.Data.Models.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Currency
    {
        public Currency()
        {
            this.Details = new HashSet<Detail>();
        }

        public int Id { get; init; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "The currency code has to be exactly 3 characters.")]
        public string Code { get; init; }

        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal ExchangeRate { get; set; }

        public IEnumerable<Detail> Details { get; set; }
    }
}
