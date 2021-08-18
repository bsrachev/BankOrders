namespace BankOrders.Services.Templates
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Details;
    using BankOrders.Models.Templates;
    using BankOrders.Services.Currencies;
    using BankOrders.Services.Details;
    using BankOrders.Services.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TemplateService : ITemplateService
    {
        private readonly ICurrencyService currencyService;
        private readonly IDetailService detailService;
        private readonly BankOrdersDbContext data;

        public TemplateService(
            ICurrencyService currencyService,
            IDetailService detailService,
            BankOrdersDbContext data)
        {
            this.currencyService = currencyService;
            this.detailService = detailService;
            this.data = data; 
        }

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

        public void Delete(int templateId)
        {
            var template = this.data.Templates.Find(templateId);

            this.data.Templates.Remove(template);

            this.data.SaveChanges();
        }

        public TemplateDetailListingViewModel GetTemplateWithEveryDetail(int templateId, int? editDetailId)
        {
            var query = new TemplateDetailListingViewModel();

            var template = GetTemplateInfo(templateId);

            var templatesDetailsQuery = this.detailService.GetDetails(template.RefNumber);

            var templatesDetailsList = new List<DetailFormModel>();

            foreach (var od in templatesDetailsQuery)
            {
                templatesDetailsList.Add(new DetailFormModel
                {
                    Account = od.Account,
                    Branch = od.Branch,
                    AccountType = od.AccountType,
                    CostCenter = od.CostCenter,
                    CurrencyId = od.CurrencyId,
                    DetailId = od.Id,
                    Project = od.Project,
                    Reason = od.Reason,
                    Sum = od.Sum,
                    SumBGN = od.SumBGN,
                    Currencies = this.currencyService.GetCurrencies(),
                    OrderSystem = template.System
                });
            }

            query.Id = template.Id;
            query.EditDetailId = editDetailId;
            query.Name = template.Name;
            query.RefNumber = template.RefNumber;
            query.TimesUsed = template.TimesUsed;
            query.System = template.System;
            query.UserCreateId = template.UserCreateId;
            query.Details = templatesDetailsList;
            query.Currencies = this.currencyService.GetCurrencies();

            return query;
        }
    }
}
