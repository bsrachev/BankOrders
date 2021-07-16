namespace BankOrders.Services
{
    using BankOrders.Models.Orders;
    
    using System.Collections.Generic;

    public interface IOrdersService
    {
        ICollection<string> ValidateOrder(CreateOrderFormModel model);
    }
}
