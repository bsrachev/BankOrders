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

        public bool IsUserCreate(int orderId, string userId);

        public bool IsUserApprove(int orderId, string userId);

        public bool IsUserAccountant(int orderId, string userId);

        public bool ChangeStatus(int orderId, string userId, OrderStatus status);
    }
}
