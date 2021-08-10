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

    public class TemplatesController : Controller
    {
        private readonly ITemplateService templateService;
        private readonly IDetailService detailService;
        private readonly IUserService userService;
        private readonly BankOrdersDbContext data;

        public TemplatesController(
            ITemplateService templateService,
            IDetailService detailService,
            IUserService userService,
            BankOrdersDbContext data)
        {
            this.templateService = templateService;
            this.detailService = detailService;
            this.userService = userService;
            this.data = data;
        }

        [Authorize]
        public IActionResult All()
        {
            /*
            var templatesQuery = this.data
                .Templates
                .AsQueryable();

            var templates = templatesQuery
                .Select(c => new TemplateListingViewModel
                {
                    //Id = c.Id,
                    RefNumber = c.RefNumber,
                    AccountingDate = c.AccountingDate,
                    System = c.System,
                    UserCreate = c.UserCreate,
                    UserApprove = c.UserApprove,
                    UserAccountant = c.UserAccountant,
                    UserApproveAccounting = c.UserApproveAccounting,
                })
                .ToList();*/

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

            var template = this.templateService.Details(templateId);

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
                    Currency = od.Currency,
                    DetailId = od.Id,
                    Project = od.Project,
                    Reason = od.Reason,
                    Sum = od.Sum,
                    SumBGN = od.SumBGN,
                    Currencies = this.data.Currencies,
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
            query.Currencies = this.data.Currencies;

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

            if (editDetailId == null)
            {
                var template = this.templateService.Details(templateId);

                var templateDetail = new Detail
                {
                    Account = templateDetailModel.Account,
                    AccountingNumber = templateDetailModel.AccountingNumber,
                    AccountType = templateDetailModel.AccountType,
                    Branch = templateDetailModel.Branch,
                    CostCenter = templateDetailModel.CostCenter,
                    Currency = templateDetailModel.Currency,
                    OrderOrTemplateRefNum = template.RefNumber,
                    Project = templateDetailModel.Project,
                    Reason = templateDetailModel.Reason,
                    Sum = templateDetailModel.Sum,
                    SumBGN = templateDetailModel.SumBGN
                };

                this.data.Details.Add(templateDetail);
            }
            else
            {
                var templateDetail = this.data.Details.Find(editDetailId);

                templateDetail.Account = templateDetailModel.Account;
                templateDetail.AccountingNumber = templateDetailModel.AccountingNumber;
                templateDetail.AccountType = templateDetailModel.AccountType;
                templateDetail.Branch = templateDetailModel.Branch;
                templateDetail.CostCenter = templateDetailModel.CostCenter;
                templateDetail.Currency = templateDetailModel.Currency;
                templateDetail.Project = templateDetailModel.Project;
                templateDetail.Reason = templateDetailModel.Reason;
                templateDetail.Sum = templateDetailModel.Sum;
                templateDetail.SumBGN = templateDetailModel.SumBGN;
            }

            this.data.SaveChanges();

            return this.Redirect($"/Templates/Details?templateId={@templateId}");
        }
    }
}
