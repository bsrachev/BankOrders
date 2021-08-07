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
    using BankOrders.Models.OrderDetails;

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

        [Authorize]
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
                    Status = c.Status
                })
                .ToList();

            return this.View(orders);
        }

        [HttpPost]
        public IActionResult All(OrderSearchFormModel searchModel)
        {
            var ordersQuery = this.data.Orders.AsQueryable();

            if (searchModel.RefNumber != null)
            {
                ordersQuery = ordersQuery.Where(x => x.RefNumber == int.Parse(searchModel.RefNumber));
            }

            if (searchModel.Status != null)
            {
                ordersQuery = ordersQuery.Where(x => x.Status == (OrderStatus)Enum.Parse(typeof(OrderStatus), searchModel.Status));
            }

            if (searchModel.AccountingDateFrom != null)
            {
                var accDateFrom = DateTime.ParseExact(searchModel.AccountingDateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                ordersQuery = ordersQuery.Where(x => x.AccountingDate >= accDateFrom);
            }

            if (searchModel.AccountingDateTo != null)
            {
                var accDateTo = DateTime.ParseExact(searchModel.AccountingDateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                ordersQuery = ordersQuery.Where(x => x.AccountingDate <= accDateTo);
            }

            if (searchModel.UserCreate != null)
            {
                ordersQuery = ordersQuery.Where(x => x.UserCreate == searchModel.UserCreate);
            }

            if (searchModel.UserApprove != null)
            {
                ordersQuery = ordersQuery.Where(x => x.UserApprove == searchModel.UserApprove);
            }

            if (searchModel.UserAccountant != null)
            {
                ordersQuery = ordersQuery.Where(x => x.UserAccountant == searchModel.UserAccountant);
            }

            if (searchModel.UserApproveAccounting != null)
            {
                ordersQuery = ordersQuery.Where(x => x.UserApproveAccounting == searchModel.UserApproveAccounting);
            }

            if (searchModel.System != null)
            {
                ordersQuery = ordersQuery.Where(x => x.System == (OrderSystem)Enum.Parse(typeof(OrderSystem), searchModel.System));
            }

            if (searchModel.IsLocked != null)
            {
                //ordersQuery = ordersQuery.Where(x => x.RefNumber == int.Parse(searchModel.RefNumber));
            }

            var orders = ordersQuery
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

        [Authorize]
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

        [Authorize]
        public IActionResult Details(int orderId, int? editDetailId) // public IActionResult Create() / async Task<IActionResult>
        {
            var query = new OrderDetailListingViewModel();
            //[FromQuery] OrderDetailListingViewModel query

            var order = this.ordersService.Details(orderId);

            /*var order = this.data
                .Orders
                .Where(c => c.Id == id)
                .FirstOrDefault();*/

            var ordersDetailsQuery = this.data.OrderDetails.AsQueryable();

            ordersDetailsQuery = ordersDetailsQuery.Where(x => x.OrderOrTemplateRefNum == order.RefNumber);

            var ordersDetailsList = new List<OrderDetailFormModel>();

            foreach (var od in ordersDetailsQuery)
            {
                ordersDetailsList.Add(new OrderDetailFormModel
                {
                    Account = od.Account,
                    AccountingNumber = od.AccountingNumber,
                    Branch = od.Branch,
                    AccountType = od.AccountType,
                    CostCenter = od.CostCenter,
                    Currency = od.Currency,
                    OrderDetailId = od.Id,
                    Project = od.Project,
                    Reason = od.Reason,
                    Sum = od.Sum,
                    SumBGN = od.SumBGN,
                    ExchangeRates = this.data.ExchangeRates,
                    OrderSystem = order.System
                });
            }

            query.Id = order.Id;
            query.EditDetailId = editDetailId;
            query.AccountingDate = order.AccountingDate;
            query.RefNumber = order.RefNumber;
            query.Status = order.Status;
            query.System = order.System;
            query.UserCreate = order.UserCreate;
            query.OrderDetails = ordersDetailsList;//ordersDetailsQuery.ToList();
            query.ExchangeRates = this.data.ExchangeRates;

            /*if (editDetailId != null)
            {
                query.EditDetailId = editDetailId;
            }*/

            return View(query);
        }

        [HttpPost]
        public IActionResult Details(OrderDetailFormModel orderDetailModel, int orderId, int? editDetailId) // public IActionResult Create() / async Task<IActionResult>
        {
            /*if (this.ordersService.Details(orderId).UserCreate == this.User.Identity.Name)
            {
                this.ModelState.AddModelError("CustomError", "Cannot appove an order that you have created.");
            }*/

            if (!ModelState.IsValid)
            {
                return this.View(orderDetailModel);
            }

            if (editDetailId == null)
            {
                var order = this.ordersService.Details(orderId);

                var orderDetail = new OrderDetail
                {
                    Account = orderDetailModel.Account,
                    AccountingNumber = orderDetailModel.AccountingNumber,
                    AccountType = orderDetailModel.AccountType,
                    Branch = orderDetailModel.Branch,
                    CostCenter = orderDetailModel.CostCenter,
                    Currency = orderDetailModel.Currency,
                    OrderOrTemplateRefNum = order.RefNumber,
                    Project = orderDetailModel.Project,
                    Reason = orderDetailModel.Reason,
                    Sum = orderDetailModel.Sum,
                    SumBGN = orderDetailModel.SumBGN
                };

                this.data.OrderDetails.Add(orderDetail);
            }
            else
            {
                var orderDetail = this.data.OrderDetails.Find(editDetailId);

                orderDetail.Account = orderDetailModel.Account;
                orderDetail.AccountingNumber = orderDetailModel.AccountingNumber;
                orderDetail.AccountType = orderDetailModel.AccountType;
                orderDetail.Branch = orderDetailModel.Branch;
                orderDetail.CostCenter = orderDetailModel.CostCenter;
                orderDetail.Currency = orderDetailModel.Currency;
                orderDetail.Project = orderDetailModel.Project;
                orderDetail.Reason = orderDetailModel.Reason;
                orderDetail.Sum = orderDetailModel.Sum;
                orderDetail.SumBGN = orderDetailModel.SumBGN;
            }

            this.data.SaveChanges();

            return this.Redirect($"/Orders/Details?orderId={@orderId}");
        }

        public IActionResult SendForApproval(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.ordersService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForApproval);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Approve(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            

            if (this.ordersService.IsUserCreate(id, this.User.Identity.Name))
            {
                this.ModelState.AddModelError("CustomError", "Cannot appove an order that you have created.");
            }

            if (!ModelState.IsValid)
            {
                //return this.View(Details(id));
                //return RedirectToAction(nameof(Details(id)));
            }

            var changeStatus = this.ordersService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForPosting);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ForCorrection(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.ordersService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForCorrection);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult SendForPostingApproval(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.ordersService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForPostingApproval);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ForPostingCorrection(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.ordersService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForPostingCorrection);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ApprovePosting(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.ordersService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.Finished);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
