namespace BankOrders.Services
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using BankOrders.Services.Orders;

    using System.Collections.Generic;

    public interface IOrdersService
    {
        ICollection<string> ValidateOrder(CreateOrderFormModel model);

        OrderDetailsServiceModel Details(int carId);

        //OrderDetailListingViewModel CurrentOrderDetails(int currentPage, int ordersPerPage);

        public bool ChangeStatus(int id, OrderStatus status);
    }
}
