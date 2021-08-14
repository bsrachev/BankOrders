using System.ComponentModel.DataAnnotations;

namespace BankOrders.Models.Templates
{
    public class CreateTemplateFormModel
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string System { get; init; }
    }
}
