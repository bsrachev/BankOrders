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

    using static WebConstants;

    public class TemplatesController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly ITemplateService templateService;
        private readonly IDetailService detailService;

        public TemplatesController(
            ICurrencyService currencyService,
            ITemplateService templateService,
            IDetailService detailService)
        {
            this.currencyService = currencyService;
            this.templateService = templateService;
            this.detailService = detailService;
        }

        [Authorize]
        public IActionResult All([FromQuery] AllTemplatesQueryModel query)
        {
            query.Templates = this.templateService.GetAllTemplates();

            return this.View(query);
        }

        [HttpPost]
        public IActionResult All(TemplateSearchFormModel searchModel)
        {
            var templates = this.templateService.GetAllTemplates(searchModel);

            return this.View(new AllTemplatesQueryModel { Templates = templates });
        }

        [Authorize]
        public IActionResult Details(int templateId, int? editDetailId)
        {
            var query = new TemplateDetailListingViewModel();

            var template = this.templateService.GetTemplateInfo(templateId);

            var templatesDetailsQuery = this.detailService.GetDetails(template.RefNumber);

            var templatesDetailsList = new List<DetailFormModel>();

            foreach (var od in templatesDetailsQuery)
            {
                templatesDetailsList.Add(new DetailFormModel
                {
                    Account = od.Account,
                    //AccountingNumber = od.AccountingNumber,
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

            return View(query);
        }

        [HttpPost]
        public IActionResult Details(DetailFormModel templateDetailModel, int templateId, int? editDetailId) // public IActionResult Create() / async Task<IActionResult>
        {
            /*if (this.templateService.Details(templateId).UserCreateId == this.User.Identity.Name)
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
