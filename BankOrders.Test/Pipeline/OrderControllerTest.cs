namespace BankOrders.Test.Pipeline
{
    using BankOrders.Controllers;
    using BankOrders.Models.Orders;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class OrderControllerTest
    {
        [Fact]
        public void GetCreateShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Orders/Create")
                    .WithUser())
                .To<OrdersController>(c => c.Create())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
    }
}
