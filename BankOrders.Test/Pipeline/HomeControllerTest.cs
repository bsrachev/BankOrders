namespace BankOrders.Test.Pipeline
{
    using System.Collections.Generic;
    using BankOrders.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void HomeShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Index")
                .To<HomeController>(c => c.Index())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void PrivacyShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Privacy")
                .To<HomeController>(c => c.Privacy())
                .Which()
                .ShouldReturn()
                .View();
    }
}
