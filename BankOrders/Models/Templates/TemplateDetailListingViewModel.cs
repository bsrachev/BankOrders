namespace BankOrders.Models.Templates
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Details;
    using BankOrders.Services.Currencies.Models;
    
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TemplateDetailListingViewModel
    {
        public int Id { get; set; }

        public int? EditDetailId { get; set; }

        public int RefNumber { get; set; }

        public string Name { get; set; }

        public OrderSystem System { get; set; }

        [Display(Name = "Created by")]
        public string UserCreateId { get; set; }

        [Display(Name = "Times Used")]
        public int TimesUsed { get; set; }

        public ICollection<DetailFormModel> Details { get; set; }

        public IEnumerable<CurrencyServiceModel> Currencies { get; set; }
    }
}
