namespace BankOrders.Services.Templates
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Templates;
    using BankOrders.Services.Orders;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TemplateService : ITemplateService
    {
        private readonly BankOrdersDbContext data;

        public TemplateService(BankOrdersDbContext data)
            => this.data = data;

        public IEnumerable<TemplateServiceModel> GetAllTemplates(TemplateSearchFormModel searchModel = null)
        {
            var templatesQuery = this.data.Templates.AsQueryable();

            if (searchModel != null)
            {
                if (searchModel.RefNumber != null)
                {
                    templatesQuery = templatesQuery.Where(x => x.RefNumber == int.Parse(searchModel.RefNumber));
                }

                if (searchModel.Name != null)
                {
                    templatesQuery = templatesQuery.Where(x => x.Name.Contains(searchModel.Name));
                }

                if (searchModel.UserCreateId != null)
                {
                    templatesQuery = templatesQuery.Where(x => x.UserCreateId == searchModel.UserCreateId);
                }

                if (searchModel.System != null)
                {
                    templatesQuery = templatesQuery.Where(x => x.System == (OrderSystem)Enum.Parse(typeof(OrderSystem), searchModel.System));
                }

                if (searchModel.TimesUsed != null)
                {
                    templatesQuery = templatesQuery.Where(x => x.TimesUsed == int.Parse(searchModel.TimesUsed));
                }
            }

            return templatesQuery
                .OrderByDescending(c => c.RefNumber)
                .Select(c => new TemplateServiceModel
                {
                    Id = c.Id,
                    RefNumber = c.RefNumber,
                    Name = c.Name,
                    System = c.System,
                    UserCreateId = c.UserCreateId,
                    TimesUsed = c.TimesUsed,
                })
                .ToList();
        }

        public TemplateServiceModel GetTemplateInfo(int id)
            => this.data
                .Templates
                .Where(c => c.Id == id)
                .Select(c => new TemplateServiceModel
                {
                    Id = c.Id,
                    RefNumber = c.RefNumber,
                    Name = c.Name,
                    System = c.System,
                    UserCreateId = c.UserCreateId,
                    TimesUsed = c.TimesUsed
                })
                .FirstOrDefault();

        public IEnumerable<TemplateServiceModel> GetAllTemplatesBySystem(OrderSystem system)
            => this.data
                .Templates
                .Where(c => c.System == system)
                .Select(c => new TemplateServiceModel
                {
                    Id = c.Id,
                    RefNumber = c.RefNumber,
                    Name = c.Name,
                    System = c.System,
                    UserCreateId = c.UserCreateId,
                    TimesUsed = c.TimesUsed
                })
                .OrderBy(n => n.Name)
                .ToList();

        public int Create(string name, string system, string userId)
        {
            var template = new Template
            {
                Name = name,
                System = (OrderSystem)Enum.Parse(typeof(OrderSystem), system, true),
                UserCreateId = userId
            };

            this.data.Templates.Add(template);

            this.data.SaveChanges();

            return template.Id;
        }
    }
}
