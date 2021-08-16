namespace BankOrders.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BankOrders.Infrastructure;
    using BankOrders.Models.Templates;
    using BankOrders.Services.Details;
    using BankOrders.Services.Templates;

    using static WebConstants;

    public class TemplatesController : AdminController
    {
        private readonly ITemplateService templateService;
        private readonly IDetailService detailService;

        public TemplatesController(ITemplateService templateService, IDetailService detailService)
        {
            this.templateService = templateService;
            this.detailService = detailService;
        }

        public IActionResult Create()
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

            int templateId = this.templateService.Create(templateModel.Name, templateModel.System, this.User.Id());

            return this.Redirect($"/Templates/Details/?templateId={templateId}");
        }

        public IActionResult Delete(int id)
        {
            this.templateService.Delete(id);

            return this.Redirect($"/Templates/All");
        }

        public IActionResult DeleteDetail(int templateId, int detailId)
        {
            this.detailService.DeleteDetail(detailId);

            return this.Redirect($"/Templates/Details?templateId={@templateId}");
        }
    }
}
