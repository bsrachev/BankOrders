namespace BankOrders.Test.Controllers
{
    using System.Collections.Generic;
    using BankOrders.Areas.Admin.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CurrenciesControllerTest
    {
        [Fact]
        public void CurrenciesIndexShouldReturnView()
            => MyController<CurrenciesController>
                .Instance()
                .Calling(c => c.Index())
                .ShouldReturn()
                .View();
    }
}
