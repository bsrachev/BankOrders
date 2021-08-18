namespace BankOrders.Test.Routing
{
    using System.Collections.Generic;
    using BankOrders.Areas.Admin.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class CurrenciesControllerTest
    {
        [Fact]
        public void CurrenciesIndexRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Currencies")
                .To<CurrenciesController>(c => c.Index());
    }
}
