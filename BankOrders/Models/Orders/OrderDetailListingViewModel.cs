namespace BankOrders.Models.Orders
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Details;
    using BankOrders.Services.Currencies.Models;
    using BankOrders.Services.Templates;
    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderDetailListingViewModel
    {
        public int Id { get; set; }

        public int? EditDetailId { get; set; }

        public int RefNumber { get; set; }

        [Display(Name = "Accounting Date")]
        public DateTime AccountingDate { get; set; }

        public OrderSystem System { get; set; }

        [Display(Name = "Created by")]
        public string UserCreateId { get; set; }

        public OrderStatus Status { get; set; }

        public IEnumerable<DetailFormModel> Details { get; set; }

        public IEnumerable<CurrencyServiceModel> Currencies { get; set; }

        public int TemplateId { get; init; }

        public IEnumerable<TemplateServiceModel> Templates { get; set; }

        [Display(Name = "Posting Number")]
        public int PostingNumber { get; set; }
    }
}
