using System.ComponentModel.DataAnnotations;

namespace BankOrders.Models.Orders
{
    public class CreateOrderFormModel
    {
        [Required(ErrorMessage = "Cannot be null")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "MyCustomMessage")]
        public string AccountingDate { get; init; } // init

        public string System { get; init; }
    }
}
