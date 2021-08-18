namespace BankOrders.Test.Controllers
{
    using BankOrders.Controllers;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Globalization;
    using System.Linq;
    using Xunit;

    public class OrderControllerTest
    {
        [Fact]
        public void GetCreateOrderShouldBeForAuthorizedUsersAndReturnView()
            => MyController<OrdersController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(OrderSystem.Internal)]
        public void PostCreateOrderShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            OrderSystem system)
            => MyController<OrdersController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Create(new CreateOrderFormModel
                {
                    AccountingDate = "2017-01-18",
                    System = system.ToString()
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Order>(orders => orders
                        .Any(d =>
                            d.AccountingDate == new DateTime(2017, 1, 18) &&
                            d.System == system &&
                            d.UserCreateId == TestUser.Identifier)))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .ToUrl("/Orders/Details/?orderId=1"));
    }
}