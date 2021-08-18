namespace BankOrders.Controllers.Api
{
    using BankOrders.Services.Details;
    using BankOrders.Services.Templates;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Collections.Generic;

    [ApiController]
    [Route("api/templates")]
    public class TemplatesApiController : ControllerBase
    {
        private readonly ITemplateService templateService;
        private readonly IDetailService detailService;

        public TemplatesApiController(ITemplateService templateService, IDetailService detailService)
        { 
            this.templateService = templateService;
            this.detailService = detailService;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<DetailsServiceModel> GetDetails(int templateId)
        {
            var template = this.templateService.GetTemplateInfo(templateId);

            return this.detailService.GetDetails(template.RefNumber);
        }
    }
}
