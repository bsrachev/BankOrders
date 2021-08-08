namespace BankOrders.Models.Templates
{
    using BankOrders.Data.Models.Enums;

    public class TemplateListingViewModel
    {
        public int Id { get; set; }

        public int RefNumber { get; set; }

        public string Name { get; set; }

        public OrderSystem System { get; set; }

        public string UserCreate { get; set; }

        public int TimesUsed { get; set; }
    }
}
