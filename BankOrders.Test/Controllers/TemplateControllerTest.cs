namespace BankOrders.Test.Controllers
{
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using BankOrders.Models.Templates;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    public class TemplateControllerTest
    {
        [Fact]
        public void GetCreateTemplateShouldBeForAuthorizedUsersAndReturnView()
            => MyController<BankOrders.Areas.Admin.Controllers.TemplatesController>
                .Instance()
                .WithUser(TestUser.Username, "Administration")
                .Calling(c => c.Create())
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Template with a name", OrderSystem.Internal)]
        public void PostCreateTemplateShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string name,
            OrderSystem system)
            => MyController<BankOrders.Areas.Admin.Controllers.TemplatesController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Create(new CreateTemplateFormModel
                {
                    Name = name,
                    System = system.ToString()
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Template>(templates => templates
                        .Any(d =>
                            d.Name == name &&
                            d.System == system &&
                            d.UserCreateId == TestUser.Identifier)))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .ToUrl("/Templates/Details/?templateId=1"));
    }
}