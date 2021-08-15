namespace BankOrders.Models.Orders
{
    using BankOrders.Services.Templates;
    using System.Collections.Generic;

    public class AllTemplatesQueryModel
    {
        public IEnumerable<TemplateServiceModel> Templates { get; set; }
    }
}
