namespace BankOrders.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Services.Orders;
    using BankOrders.Models.Orders;
    using BankOrders.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BankOrders.Models.Details;
    using BankOrders.Services.Users;
    using BankOrders.Services.Details;
    using BankOrders.Services.Templates;

    using static Data.DataConstants.Errors;
    using static WebConstants;

    public class OrdersController : Controller
    {
        private readonly ITemplateService templateService;
        private readonly IOrderService orderService;
        private readonly IDetailService detailService;
        private readonly IUserService userService;
        private readonly BankOrdersDbContext data;

        public OrdersController(
            ITemplateService templateService,
            IOrderService orderService,
            IDetailService detailService,
            IUserService userService,
            BankOrdersDbContext data)
        {
            this.templateService = templateService;
            this.orderService = orderService;
            this.detailService = detailService;
            this.userService = userService;
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
                .Select(c => new OrderServiceModel
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
        [Authorize]
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
                .Select(c => new OrderServiceModel
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
        public IActionResult Details(int orderId, int? editDetailId, string errText = null) // public IActionResult Create() / async Task<IActionResult>
        {
            if (errText != null)
            {
                this.ModelState.AddModelError(String.Empty, errText);
            }

            var query = new OrderDetailListingViewModel();
            //[FromQuery] DetailListingViewModel query

            var order = this.orderService.GetOrderInfo(orderId);

            /*var order = this.data
                .Orders
                .Where(c => c.Id == id)
                .FirstOrDefault();*/

            var ordersDetailsQuery = this.data.Details.AsQueryable();

            ordersDetailsQuery = ordersDetailsQuery.Where(x => x.OrderOrTemplateRefNum == order.RefNumber);

            var ordersDetailsList = new List<DetailFormModel>();

            foreach (var od in ordersDetailsQuery)
            {
                ordersDetailsList.Add(new DetailFormModel
                {
                    Account = od.Account,
                    AccountingNumber = od.AccountingNumber,
                    Branch = od.Branch,
                    AccountType = od.AccountType,
                    CostCenter = od.CostCenter,
                    Currency = od.Currency,
                    DetailId = od.Id,
                    Project = od.Project,
                    Reason = od.Reason,
                    Sum = od.Sum,
                    SumBGN = od.SumBGN,
                    Currencies = this.data.Currencies,
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
            query.Details = ordersDetailsList;//ordersDetailsQuery.ToList();
            query.Currencies = this.data.Currencies;
            query.Templates = this.templateService.AllTemplatesBySystem(query.System);

            /*if (editDetailId != null)
            {
                query.EditDetailId = editDetailId;
            }*/

            return View(query);
        }

        [HttpPost]
        public IActionResult Details(DetailFormModel detailModel, int orderId, int? editDetailId) // public IActionResult Create() / async Task<IActionResult>
        {
            /*if (this.orderService.Details(orderId).UserCreate == this.User.Identity.Name)
            {
                this.ModelState.AddModelError("CustomError", "Cannot appove an order that you have created.");
            }*/

            if (!ModelState.IsValid)
            {
                return this.View(detailModel);
            }

            if (editDetailId == null)
            {
                var order = this.orderService.GetOrderInfo(orderId);

                var detail = new Detail
                {
                    Account = detailModel.Account,
                    AccountingNumber = detailModel.AccountingNumber,
                    AccountType = detailModel.AccountType,
                    Branch = detailModel.Branch,
                    CostCenter = detailModel.CostCenter,
                    Currency = detailModel.Currency,
                    OrderOrTemplateRefNum = order.RefNumber,
                    Project = detailModel.Project,
                    Reason = detailModel.Reason,
                    Sum = detailModel.Sum,
                    SumBGN = detailModel.SumBGN
                };

                this.data.Details.Add(detail);
            }
            else
            {
                var detail = this.data.Details.Find(editDetailId);

                detail.Account = detailModel.Account;
                detail.AccountingNumber = detailModel.AccountingNumber;
                detail.AccountType = detailModel.AccountType;
                detail.Branch = detailModel.Branch;
                detail.CostCenter = detailModel.CostCenter;
                detail.Currency = detailModel.Currency;
                detail.Project = detailModel.Project;
                detail.Reason = detailModel.Reason;
                detail.Sum = detailModel.Sum;
                detail.SumBGN = detailModel.SumBGN;
            }

            this.data.SaveChanges();

            return this.Redirect($"/Orders/Details?orderId={@orderId}");
        }

        public IActionResult SendForApproval(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var order = this.orderService.GetOrderInfo(id);

            var details = this.detailService.GetDetails(order.RefNumber);

            if (details.Count == 0)
            {
                TempData[GlobalErrorKey] = NoDetailsError;

                return RedirectToAction(nameof(Details), new { orderId = id });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForApproval);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Approve(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            //var shmest = this.User.Id();

            if (this.userService.IsUserCreate(id, this.User.Identity.Name))
            {
                var errText = UserCreateAndUserApproveCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id, errText = errText });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForPosting);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ForCorrection(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.orderService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForCorrection);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult SendForPostingApproval(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            if (this.userService.IsUserApprove(id, this.User.Identity.Name))
            {
                var errText = UserApproveAndUserAccountantCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id, errText = errText });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForPostingApproval);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ForPostingCorrection(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.orderService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.ForPostingCorrection);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ApprovePosting(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            if (this.userService.IsUserAccountant(id, this.User.Identity.Name))
            {
                var errText = UserAccountantAndUserApproveAccountingCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id, errText = errText });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Identity.Name, OrderStatus.Finished);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
