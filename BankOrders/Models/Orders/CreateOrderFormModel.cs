using System.ComponentModel.DataAnnotations;

namespace BankOrders.Models.Orders
{
    public class CreateOrderFormModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Accounting Date")]
        public string AccountingDate { get; init; }

        [Required]
        public string System { get; init; }
    }
}
