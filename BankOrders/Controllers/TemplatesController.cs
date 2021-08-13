namespace BankOrders.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Services.Templates;
    using BankOrders.Models.Orders;
    using BankOrders.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BankOrders.Models.Details;
    using BankOrders.Models.Templates;
    using BankOrders.Services.Users;
    using BankOrders.Services.Details;
    using BankOrders.Services.Currencies;

    public class TemplatesController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly ITemplateService templateService;
        private readonly IDetailService detailService;
        private readonly IUserService userService;
        private readonly BankOrdersDbContext data;

        public TemplatesController(
            ICurrencyService currencyService,
            ITemplateService templateService,
            IDetailService detailService,
            IUserService userService,
            BankOrdersDbContext data)
        {
            this.currencyService = currencyService;
            this.templateService = templateService;
            this.detailService = detailService;
            this.userService = userService;
            this.data = data;
        }

        [Authorize]
        public IActionResult All()
        {
            var templates = this.data
                .Templates
                .OrderByDescending(c => c.RefNumber)
                .Select(c => new TemplateServiceModel
                {
                    Id = c.Id,
                    RefNumber = c.RefNumber,
                    Name = c.Name,
                    System = c.System,
                    UserCreate = c.UserCreate,
                    TimesUsed = c.TimesUsed
                })
                .ToList();

            return this.View(templates);
        }

        [HttpPost]
        public IActionResult All(TemplateSearchFormModel searchModel)
        {
            var templatesQuery = this.data.Templates.AsQueryable();

            if (searchModel.RefNumber != null)
            {
                templatesQuery = templatesQuery.Where(x => x.RefNumber == int.Parse(searchModel.RefNumber));
            }

            if (searchModel.Name != null)
            {
                templatesQuery = templatesQuery.Where(x => x.Name == searchModel.Name);
            }

            if (searchModel.UserCreate != null)
            {
                templatesQuery = templatesQuery.Where(x => x.UserCreate == searchModel.UserCreate);
            }

            if (searchModel.System != null)
            {
                templatesQuery = templatesQuery.Where(x => x.System == (OrderSystem)Enum.Parse(typeof(OrderSystem), searchModel.System));
            }

            var templates = templatesQuery
                .OrderByDescending(c => c.RefNumber)
                .Select(c => new TemplateServiceModel
                {
                    Id = c.Id,
                    RefNumber = c.RefNumber,
                    Name = c.Name,
                    System = c.System,
                    UserCreate = c.UserCreate,
                    TimesUsed = c.TimesUsed
                })
                .ToList();

            return this.View(templates);
        }

        [Authorize]
        public IActionResult Create() // public IActionResult Create() / async Task<IActionResult>
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateTemplateFormModel templateModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(templateModel);
            }

            /*if (templateModel.AccountingDate > 2015)
            {
                var template = .....
            }
            else
            {
                ......
            }*/

            var template = new Template
            {
                //RefNumber = 10000001,
                Name = templateModel.Name,
                System = (OrderSystem)Enum.Parse(typeof(OrderSystem), templateModel.System, true),
                UserCreate = this.User.Identity.Name
            };

            this.data.Templates.Add(template);

            this.data.SaveChanges();

            return this.Redirect("/Templates/All");
        }

        [Authorize]
        public IActionResult Details(int templateId, int? editDetailId) // public IActionResult Create() / async Task<IActionResult>
        {
            var query = new TemplateDetailListingViewModel();
            //[FromQuery] TemplateDetailListingViewModel query

            var template = this.templateService.GetTemplateInfo(templateId);

            /*var template = this.data
                .Templates
                .Where(c => c.Id == id)
                .FirstOrDefault();*/

            var templatesDetailsQuery = this.data.Details.AsQueryable();

            templatesDetailsQuery = templatesDetailsQuery.Where(x => x.OrderOrTemplateRefNum == template.RefNumber);

            var templatesDetailsList = new List<DetailFormModel>();

            foreach (var od in templatesDetailsQuery)
            {
                templatesDetailsList.Add(new DetailFormModel
                {
                    Account = od.Account,
                    AccountingNumber = od.AccountingNumber,
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
            query.UserCreate = template.UserCreate;
            query.Details = templatesDetailsList;//templatesDetailsQuery.ToList();
            query.Currencies = this.currencyService.GetCurrencies();

            /*if (editDetailId != null)
            {
                query.EditDetailId = editDetailId;
            }*/

            return View(query);
        }

        [HttpPost]
        public IActionResult Details(DetailFormModel templateDetailModel, int templateId, int? editDetailId) // public IActionResult Create() / async Task<IActionResult>
        {
            /*if (this.templateService.Details(templateId).UserCreate == this.User.Identity.Name)
            {
                this.ModelState.AddModelError("CustomError", "Cannot appove an template that you have created.");
            }*/

            if (!ModelState.IsValid)
            {
                return this.View(templateDetailModel);
            }

            var template = this.templateService.GetTemplateInfo(templateId);

            if (editDetailId == null)
            {
                this.detailService.AddDetail(templateDetailModel.Account,
                                             templateDetailModel.AccountType,
                                             templateDetailModel.Branch,
                                             templateDetailModel.CostCenter,
                                             templateDetailModel.CurrencyId,
                                             template.RefNumber,
                                             templateDetailModel.Project,
                                             templateDetailModel.Reason,
                                             templateDetailModel.Sum,
                                             templateDetailModel.SumBGN);
            }
            else
            {
                this.detailService.EditDetail(Convert.ToInt32(editDetailId),
                                              templateDetailModel.Account,
                                              templateDetailModel.AccountType,
                                              templateDetailModel.Branch,
                                              templateDetailModel.CostCenter,
                                              templateDetailModel.CurrencyId,
                                              templateDetailModel.Project,
                                              templateDetailModel.Reason,
                                              templateDetailModel.Sum,
                                              templateDetailModel.SumBGN);
            }

            return this.Redirect($"/Templates/Details?templateId={@templateId}");
        }
    }
}
