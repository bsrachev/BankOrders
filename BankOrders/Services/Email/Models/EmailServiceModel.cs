using System.ComponentModel.DataAnnotations;

namespace BankOrders.Services.Email.Models
{
    public class EmailServiceModel
    {
        [Required]
        public string RecepientId { get; set; }
    }
}
