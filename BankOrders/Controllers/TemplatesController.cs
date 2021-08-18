namespace BankOrders.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using BankOrders.Services.Templates;
    using BankOrders.Models.Orders;
    using BankOrders.Models.Details;
    using BankOrders.Models.Templates;
    using BankOrders.Services.Details;
    using BankOrders.Services.Currencies;
    using BankOrders.Areas.Admin;
    
    using System;

    public class TemplatesController : Controller
    {
        private readonly ITemplateService templateService;
        private readonly IDetailService detailService;

        public TemplatesController(
            ITemplateService templateService,
            IDetailService detailService)
        {
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
        [Authorize]
        public IActionResult All(TemplateSearchFormModel searchModel)
        {
            var templates = this.templateService.GetAllTemplates(searchModel);

            return this.View(new AllTemplatesQueryModel { Templates = templates });
        }

        [Authorize]
        public IActionResult Details(int templateId, int? editDetailId)
        {
            var query = this.templateService.GetTemplateWithEveryDetail(templateId, editDetailId);

            return View(query);
        }

        [HttpPost]
        [Authorize(Roles = AdminConstants.AdministratorRoleName)]
        public IActionResult Details(DetailFormModel templateDetailModel, int templateId, int? editDetailId) // public IActionResult Create() / async Task<IActionResult>
        {
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

        [Authorize(Roles = AdminConstants.AdministratorRoleName)]
        public IActionResult DeleteDetail(int templateId, int detailId)
        {
            this.detailService.DeleteDetail(detailId);

            return this.Redirect($"/Templates/Details?templateId={@templateId}");
        }
    }
}
