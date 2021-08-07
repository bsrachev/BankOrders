namespace BankOrders.Services
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using BankOrders.Services.Orders;

    using System.Collections.Generic;

    public interface IOrderService
    {
        ICollection<string> ValidateOrder(CreateOrderFormModel model);

        OrderDetailsServiceModel Details(int carId);

        public bool ChangeStatus(int orderId, string userId, OrderStatus status);
    }
}
