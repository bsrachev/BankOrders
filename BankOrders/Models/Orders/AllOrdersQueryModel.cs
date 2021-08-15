namespace BankOrders.Models.Orders
{
    using BankOrders.Services.Orders;
    using System.Collections.Generic;

    public class AllOrdersQueryModel
    {
        public IEnumerable<OrderServiceModel> Orders { get; set; }
    }
}
