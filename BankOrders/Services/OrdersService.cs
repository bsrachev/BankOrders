namespace BankOrders.Services
{
    using System;
    using System.Collections.Generic;

    using BankOrders.Models.Orders;

    public class OrdersService : IOrdersService
    {
        public ICollection<string> ValidateOrder(CreateOrderFormModel model)
        {
            var errors = new List<string>();

            if (model.System != "Internal" && model.System != "External")
            {
                errors.Add($"Invalid payment system.");
            }

            DateTime temp;
            if (!DateTime.TryParse(model.AccountingDate, out temp)) // TODO
            {
                errors.Add($"Invalid date.");
            }

            return errors;
        }
    }
}
