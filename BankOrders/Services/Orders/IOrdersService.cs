namespace BankOrders.Services
{
    using BankOrders.Models.Orders;
    using BankOrders.Services.Orders;

    using System.Collections.Generic;

    public interface IOrdersService
    {
        ICollection<string> ValidateOrder(CreateOrderFormModel model);

        OrderDetailsServiceModel Details(int carId);
    }
}
