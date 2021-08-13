namespace BankOrders.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TemplatesController : AdminController
    {

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

            /*var templates = this.data
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
                .ToList();*/

            return this.View(null); //templates
        }
    }
}
