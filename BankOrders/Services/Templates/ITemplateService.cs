namespace BankOrders.Services.Templates
{
    using BankOrders.Data.Models.Enums;
    
    using System.Collections.Generic;

    public interface ITemplateService
    {
        TemplateServiceModel Details(int carId);

        public IEnumerable<TemplateServiceModel> AllTemplatesBySystem(OrderSystem system);
    }
}
