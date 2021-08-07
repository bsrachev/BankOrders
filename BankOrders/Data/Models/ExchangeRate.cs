namespace BankOrders.Data.Models
{
    using BankOrders.Data.Models.Enums;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExchangeRate
    {
        public int Id { get; init; }

        [Required]
        public Currency Currency { get; init; }

        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal Rate { get; set; }
    }
}
