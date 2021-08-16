namespace BankOrders.Services.Orders
{
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using BankOrders.Services.Orders;

    using System.Collections.Generic;

    public interface IOrderService
    {
        int Create(string accountingDate, string system, string userId);

        OrderServiceModel GetOrderInfo(int orderId);

        public bool ChangeStatus(int orderId, string userId, OrderStatus status);

        IEnumerable<OrderServiceModel> GetAllOrders(OrderSearchFormModel searchModel = null);
        
        void AddPostingNumber(int orderId, int postingNumber);

        void CancelOrder(int orderId);
    }
}
