namespace BankOrders.Models.Orders
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Details;
    using BankOrders.Services.Currencies.Models;
    using BankOrders.Services.Templates;
    
    using System;
    using System.Collections.Generic;

    public class OrderDetailListingViewModel
    {
        public int Id { get; set; }

        public int? EditDetailId { get; set; }

        public int RefNumber { get; set; }

        public DateTime? AccountingDate { get; set; }

        public OrderSystem System { get; set; }

        public string UserCreateId { get; set; }

        public OrderStatus Status { get; set; }

        public IEnumerable<DetailFormModel> Details { get; set; }

        public IEnumerable<CurrencyServiceModel> Currencies { get; set; }

        public int TemplateId { get; init; }

        public IEnumerable<TemplateServiceModel> Templates { get; set; }

        public int PostingNumber { get; set; }
    }
}
