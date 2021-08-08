namespace BankOrders.Services.Orders
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using BankOrders.Services.Orders;

    using System.Collections.Generic;

    public interface IOrderService
    {
        OrderServiceModel Details(int carId);

        public bool ChangeStatus(int orderId, string userId, OrderStatus status);
    }
}
