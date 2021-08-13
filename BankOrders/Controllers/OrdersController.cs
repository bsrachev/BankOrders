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
    using BankOrders.Services.Currencies;

    public class OrdersController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly ITemplateService templateService;
        private readonly IOrderService orderService;
        private readonly IDetailService detailService;
        private readonly IUserService userService;
        private readonly BankOrdersDbContext data;

        public OrdersController(
            ICurrencyService currencyService,
            ITemplateService templateService,
            IOrderService orderService,
            IDetailService detailService,
            IUserService userService,
            BankOrdersDbContext data)
        {
            this.currencyService = currencyService;
            this.templateService = templateService;
            this.orderService = orderService;
            this.detailService = detailService;
            this.userService = userService;
            this.data = data;
        }

        [Authorize]
        public IActionResult All()
        {
            var orders = this.orderService.GetAllOrders();

            return this.View(orders);
        }

        [HttpPost]
        [Authorize]
        public IActionResult All(OrderSearchFormModel searchModel)
        {
            var orders = this.orderService.GetAllOrders(searchModel);

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
            /*if (errText != null)
            {
                this.ModelState.AddModelError(String.Empty, errText);
            }*/

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
                //var currency = this.currencyService.GetCurrencyInfo(od.CurrencyId);

                ordersDetailsList.Add(new DetailFormModel
                {
                    Account = od.Account,
                    AccountingNumber = od.AccountingNumber,
                    Branch = od.Branch,
                    AccountType = od.AccountType,
                    CostCenter = od.CostCenter,
                    CurrencyId = od.CurrencyId,
                    DetailId = od.Id,
                    Project = od.Project,
                    Reason = od.Reason,
                    Sum = od.Sum,
                    SumBGN = od.SumBGN,
                    Currencies = this.currencyService.GetCurrencies(),
                    AllTemplates = this.templateService.GetAllTemplatesBySystem(order.System),
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
            query.Currencies = this.currencyService.GetCurrencies();
            query.Templates = this.templateService.GetAllTemplatesBySystem(query.System);

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

            var order = this.orderService.GetOrderInfo(orderId);

            if (editDetailId == null)
            {
                this.detailService.AddDetail(detailModel.Account,
                                             detailModel.AccountType,
                                             detailModel.Branch,
                                             detailModel.CostCenter,
                                             detailModel.CurrencyId,
                                             order.RefNumber,
                                             detailModel.Project,
                                             detailModel.Reason,
                                             detailModel.Sum,
                                             detailModel.SumBGN);
            }
            else
            {
                this.detailService.EditDetail(Convert.ToInt32(editDetailId),
                                              detailModel.Account,
                                              detailModel.AccountType,
                                              detailModel.Branch,
                                              detailModel.CostCenter,
                                              detailModel.CurrencyId,
                                              detailModel.Project,
                                              detailModel.Reason,
                                              detailModel.Sum,
                                              detailModel.SumBGN);
            }

            return this.Redirect($"/Orders/Details?orderId={@orderId}");
        }

        public IActionResult SendForApproval(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var order = this.orderService.GetOrderInfo(id);

            var details = this.detailService.GetDetails(order.RefNumber);

            if (details.Count() == 0)
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

        public IActionResult CopyDetailsFromTemplate(int orderId, int templateId)
        {
            this.detailService.CopyFromTemplate(orderId, templateId);

            return this.Redirect($"/Orders/Details?orderId={@orderId}");
        }

        public IActionResult DeleteDetail(int orderId, int detailId)
        {
            this.detailService.DeleteDetail(detailId);

            return this.Redirect($"/Orders/Details?orderId={@orderId}");
        }
    }
}
