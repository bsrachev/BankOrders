﻿namespace BankOrders.Models.Templates
{
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.OrderDetails;
    using System;
    using System.Collections.Generic;

    public class TemplateDetailListingViewModel
    {
        public int Id { get; set; }

        public int? EditDetailId { get; set; }

        public int RefNumber { get; set; }

        public string Name { get; set; }

        public OrderSystem System { get; set; }

        public string UserCreate { get; set; }

        public int TimesUsed { get; set; }

        public ICollection<OrderDetailFormModel> OrderDetails { get; set; }

        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
    }
}