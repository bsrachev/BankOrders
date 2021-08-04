using BankOrders.Services.Orders;

namespace BankOrders.Services.OrderDetails
{
    public interface IOrderDetailService
    {
        OrderDetailsServiceModel Details(int carId);
    }
}
