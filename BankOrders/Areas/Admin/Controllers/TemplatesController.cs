namespace BankOrders.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BankOrders.Services.Templates;
    using BankOrders.Infrastructure;
    using BankOrders.Models.Templates;

    using static WebConstants;

    public class TemplatesController : AdminController
    {
        private readonly ITemplateService templateService;

        public TemplatesController(ITemplateService templateService)
        {
            this.templateService = templateService;
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
    }
}
