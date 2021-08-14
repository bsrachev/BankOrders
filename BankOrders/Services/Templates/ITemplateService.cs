namespace BankOrders.Services.Templates
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Templates;

    using System.Collections.Generic;

    public interface ITemplateService
    {
        int Create(string name, string system, string userId);

        TemplateServiceModel GetTemplateInfo(int carId);

        public IEnumerable<TemplateServiceModel> GetAllTemplatesBySystem(OrderSystem system);

        IEnumerable<TemplateServiceModel> GetAllTemplates(TemplateSearchFormModel searchModel = null);
    }
}
