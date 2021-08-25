namespace BankOrders.Models.Details
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Services.Currencies.Models;
    using BankOrders.Services.Templates;
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
        public int CurrencyId { get; init; }

        [Required]
        [Display(Name = "Sum in BGN")]
        public decimal SumBGN { get; init; }

        [Required]
        public OrderSystem OrderSystem { get; set; }

        public IEnumerable<CurrencyServiceModel> Currencies { get; set; }
    }
}
