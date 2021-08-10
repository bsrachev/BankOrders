namespace BankOrders.Areas.Admin.Models.Currencies
{
    using System.ComponentModel.DataAnnotations;

    public class CurrencyFormModel
    {
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "The currency code has to be exactly 3 characters.")]
        public string Code { get; init; }
    }
}
