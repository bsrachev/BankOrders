namespace BankOrders.Models.Templates
{
    using BankOrders.Data.Models.Enums;

    using System;

    public class TemplateSearchFormModel // formerly OrderQueryServiceModel
    {
        public string RefNumber { get; set; }

        public string Name { get; set; }

        public string TimesUsed { get; set; }

        public string System { get; set; }

        public string UserCreateId { get; set; }
    }
}
