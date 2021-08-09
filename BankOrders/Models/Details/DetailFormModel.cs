namespace BankOrders.Models.Details
{
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DetailFormModel
    {
        public int DetailId { get; init; }
        
        [Required]
        public int Branch { get; init; }

        [Required]
        public int CostCenter { get; init; }

        [Required]
        public int Project { get; init; }

        [Required]
        public string Reason { get; init; }
        
        [Required]
        public string Account { get; init; }

        [Required]
        public AccountType AccountType { get; init; }

        [Required]
        public decimal Sum { get; init; }

        [Required]
        public Currency Currency { get; init; }

        [Required]
        public decimal SumBGN { get; init; }

        public int AccountingNumber { get; init; }

        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }

        [Required]
        public OrderSystem OrderSystem { get; set; }
    }
}
