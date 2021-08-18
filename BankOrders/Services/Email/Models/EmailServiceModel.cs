namespace BankOrders.Services.Email.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EmailServiceModel
    {
        public int OrderId { get; init; }

        [Required]
        [Display(Name = "Recepient employee №")]
        public string RecepientId { get; set; }
    }
}
