namespace BankOrders.Test.Routing
{
    using BankOrders.Controllers;
    using BankOrders.Models.Orders;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class OrderControllerTest
    {
        [Fact]
        public void CreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Orders/Create")
                .To<OrdersController>(c => c.Create());
    }
}
