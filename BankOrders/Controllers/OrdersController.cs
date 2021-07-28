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
        private readonly IOrdersService ordersService;
        //private readonly IUserService users;
        private readonly BankOrdersDbContext data;

        public OrdersController(
            IOrdersService ordersService,
            // IUserService users,
            BankOrdersDbContext data)
        {
            this.ordersService = ordersService;
            // this.users = users;
            this.data = data;
        }

        //[Authorize]
        public IActionResult All()
        {
            /*
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
                .ToList();*/

            var orders = this.data
                .Orders
                .OrderByDescending(c => c.RefNumber)
                .Select(c => new OrderListingViewModel
                {
                    Id = c.Id,
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
        public IActionResult Create(CreateOrderFormModel orderModel)
        {
            if (!ModelState.IsValid)
            { 
                return this.View(orderModel);
            }

            /*if (orderModel.AccountingDate > 2015)
            {
                var order = .....
            }
            else
            {
                ......
            }*/

            var order = new Order
            {
                //RefNumber = 10000001,
                AccountingDate = DateTime.ParseExact(orderModel.AccountingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                System = (OrderSystem)Enum.Parse(typeof(OrderSystem), orderModel.System, true),
                UserCreate = this.User.Identity.Name,
                Status = 0,
            };

            this.data.Orders.Add(order);

            this.data.SaveChanges();

            return this.Redirect("/Orders/All");
        }

        public IActionResult Details(int orderId, [FromQuery] OrderDetailListingViewModel query) // public IActionResult Create() / async Task<IActionResult>
        {
            //id = 4;

            var order = this.ordersService.Details(orderId);

            /*var order = this.data
                .Orders
                .Where(c => c.Id == id)
                .FirstOrDefault();*/

            var ordersDetailsQuery = this.data.OrderDetails.AsQueryable();

            ordersDetailsQuery = ordersDetailsQuery.Where(x => x.OrderId == order.Id);

            query.Id = order.Id;
            query.EditDetailId = null;
            query.AccountingDate = order.AccountingDate;
            query.RefNumber = order.RefNumber;
            query.Status = order.Status;
            query.System = order.System;
            query.UserCreate = order.UserCreate;
            query.OrderDetails = ordersDetailsQuery.ToList();

            /*if (editDetailId != null)
            {
                query.EditDetailId = editDetailId;
            }*/

            return View(query);
        }

        [HttpPost]
        public IActionResult Details(int orderId, int editDetailId, [FromQuery] OrderDetailListingViewModel query) // public IActionResult Create() / async Task<IActionResult>
        {
            //id = 4;

            var order = this.ordersService.Details(orderId);

            /*var order = this.data
                .Orders
                .Where(c => c.Id == id)
                .FirstOrDefault();*/

            var ordersDetailsQuery = this.data.OrderDetails.AsQueryable();

            ordersDetailsQuery = ordersDetailsQuery.Where(x => x.OrderId == order.Id);

            query.Id = order.Id;
            query.EditDetailId = editDetailId;
            query.AccountingDate = order.AccountingDate;
            query.RefNumber = order.RefNumber;
            query.Status = order.Status;
            query.System = order.System;
            query.UserCreate = order.UserCreate;
            query.OrderDetails = ordersDetailsQuery.ToList();

            return View(query);
        }
    }
}
