namespace BankOrders.Data.Models
{
    using BankOrders.Data.Models.Enums;

    using System.ComponentModel.DataAnnotations;

    public class ExchangeRate
    {
        public int Id { get; init; }

        [Required]
        public Currency Currency { get; init; }

        [Required]
        public decimal Rate { get; set; }
    }
}
