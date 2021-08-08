using System.ComponentModel.DataAnnotations;

namespace BankOrders.Models.Templates
{
    public class CreateTemplateFormModel
    {
        [Required(ErrorMessage = "Name cannot be empty.")]
        public string Name { get; init; }

        [Required]
        public string System { get; init; }
    }
}
