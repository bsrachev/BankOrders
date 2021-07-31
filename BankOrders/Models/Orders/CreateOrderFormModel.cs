using System.ComponentModel.DataAnnotations;

namespace BankOrders.Models.Orders
{
    public class CreateOrderFormModel
    {
        [Required(ErrorMessage = "Cannot be null")]
        //[IsDateValidationAttribute]
        public string AccountingDate { get; init; } // init

        public string System { get; init; }
    }
}
