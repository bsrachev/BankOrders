﻿namespace BankOrders.Services.Templates
{
    using BankOrders.Data;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using BankOrders.Services.Orders;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TemplateService : ITemplateService
    {
        private readonly BankOrdersDbContext data;

        public TemplateService(BankOrdersDbContext data)
            => this.data = data;

        public TemplateServiceModel Details(int id)
            => this.data
                .Templates
                .Where(c => c.Id == id)
                .Select(c => new TemplateServiceModel
                {
                    Id = c.Id,
                    RefNumber = c.RefNumber,
                    Name = c.Name,
                    System = c.System,
                    UserCreate = c.UserCreate,
                    TimesUsed = c.TimesUsed
                })
                .FirstOrDefault();
    }
}
