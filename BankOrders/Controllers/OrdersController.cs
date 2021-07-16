namespace BankOrders.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Services;
    using BankOrders.Models.Orders;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class OrdersController : Controller
    {
        //private readonly IOrdersService ordersService;
        //private readonly IUserService users;
        private readonly BankOrdersDbContext data;

        public OrdersController(
            //IOrdersService ordersService,
            // IUserService users,
            BankOrdersDbContext data)
        {
            //this.ordersService = ordersService;
            // this.users = users;
            this.data = data;
        }

        //[Authorize]
        public IActionResult All()
        {
            var ordersQuery = this.data
                .Orders
                .AsQueryable();

            var orders = ordersQuery
                .Select(c => new OrderListingViewModel
                {
                    //Id = c.Id,
                    RefNumber = c.RefNumber,
                    AccountingDate = c.AccountingDate,
                    System = c.System,
                    UserCreate = c.UserCreate,
                    UserApprove = c.UserApprove,
                    UserAccountant = c.UserAccountant,
                    UserApproveAccounting = c.UserApproveAccounting,
                })
                .ToList();

            return this.View(orders);
        }

        //[HttpGet]
        public IActionResult Create() // public IActionResult Create() / async Task<IActionResult>
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateOrderFormModel model)
        {
            /*var modelErrors = this.ordersService.ValidateOrder(model);

            if (modelErrors.Any())
            {
                // throw new ArgumentNullException(string.Format("Unable to add the order."));

                // this.TempData["Error"] = "Unable to add the order.";

                return this.Redirect("/Orders"); // TODO
            }*/

            var order = new Order
            {
                RefNumber = 10000001,
                AccountingDate = DateTime.ParseExact(model.AccountingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                System = (OrderSystem)Enum.Parse(typeof(OrderSystem), model.System, true),
                UserCreate = this.User.Identity.Name,
                Status = 0,
            };

            this.data.Orders.Add(order);

            this.data.SaveChanges();

            return this.Redirect("/Orders/All");
        }
    }
}
