namespace BankOrders.Services.OrderDetails
{
    using System.Collections.Generic;

    public interface IOrderDetailService
    {
        ICollection<OrderDetailsServiceModel> GetDetails(int orderId);
    }
}
