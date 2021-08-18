namespace BankOrders.Test.Routing
{
    using BankOrders.Areas.Admin.Controllers;
    using BankOrders.Models.Templates;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class TemplateControllerTest
    {
        [Fact]
        public void CreateRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Templates/Create")
                .To<TemplatesController>(c => c.Create());
    }
}
