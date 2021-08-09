using System.ComponentModel.DataAnnotations;

namespace BankOrders.Models.Orders
{
    public class CreateOrderFormModel
    {
        [Required]
        [Display(Name = "Accounting Date")]
        //[IsDateValidationAttribute]
        public string AccountingDate { get; init; }

        [Required]
        public string System { get; init; }
    }
}
