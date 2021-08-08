namespace BankOrders.Services.OrderDetails
{
    using BankOrders.Services.Orders;

    public interface IOrderDetailService
    {
        OrderDetailsServiceModel Details(int carId);
    }
}
