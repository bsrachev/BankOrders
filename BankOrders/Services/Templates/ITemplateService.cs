namespace BankOrders.Services.Templates
{
    using BankOrders.Data.Models.Enums;
    
    using System.Collections.Generic;

    public interface ITemplateService
    {
        TemplateServiceModel GetTemplateInfo(int carId);

        public IEnumerable<TemplateServiceModel> GetAllTemplatesBySystem(OrderSystem system);
    }
}
