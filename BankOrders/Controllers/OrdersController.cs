﻿namespace BankOrders.Controllers
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
    using BankOrders.Services.Currencies;

    using static Data.DataConstants.Errors;
    using static WebConstants;

    public class OrdersController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly ITemplateService templateService;
        private readonly IOrderService orderService;
        private readonly IDetailService detailService;
        private readonly IUserService userService;

        public OrdersController(
            ICurrencyService currencyService,
            ITemplateService templateService,
            IOrderService orderService,
            IDetailService detailService,
            IUserService userService)
        {
            this.currencyService = currencyService;
            this.templateService = templateService;
            this.orderService = orderService;
            this.detailService = detailService;
            this.userService = userService;
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

            int orderId = this.orderService.Create(orderModel.AccountingDate, orderModel.System, this.User.Id());

            return this.Redirect($"/Orders/Details/?orderId={orderId}");
        }

        [Authorize]
        public IActionResult Details(int orderId, int? editDetailId)
        {
            var query = new OrderDetailListingViewModel();

            var order = this.orderService.GetOrderInfo(orderId);

            var ordersDetailsQuery = this.detailService.GetDetails(order.RefNumber);

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
                    CurrencyId = od.CurrencyId,
                    DetailId = od.Id,
                    Project = od.Project,
                    Reason = od.Reason,
                    Sum = od.Sum,
                    SumBGN = od.SumBGN,
                    Currencies = this.currencyService.GetCurrencies(),
                    //AllTemplates = this.templateService.GetAllTemplatesBySystem(order.System),
                    OrderSystem = order.System
                });
            }

            query.Id = order.Id;
            query.EditDetailId = editDetailId;
            query.AccountingDate = order.AccountingDate;
            query.RefNumber = order.RefNumber;
            query.Status = order.Status;
            query.System = order.System;
            query.UserCreateId = order.UserCreateId;
            query.Details = ordersDetailsList;
            query.Currencies = this.currencyService.GetCurrencies();
            query.Templates = this.templateService.GetAllTemplatesBySystem(query.System);

            return View(query);
        }

        [HttpPost]
        public IActionResult Details(DetailFormModel detailModel, int orderId, int? editDetailId) // public IActionResult Create() / async Task<IActionResult>
        {
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

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForApproval);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Approve(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var shmest = this.User.Id();

            if (this.userService.IsOrderUserCreate(id, this.User.Id()))
            {
                var errText = UserCreateIdAndUserApproveIdCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id, errText = errText });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForPosting);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ForCorrection(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForCorrection);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult SendForPostingApproval(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            if (this.userService.IsUserApprove(id, this.User.Id()))
            {
                var errText = UserApproveIdAndUserPostingIdCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id, errText = errText });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForPostingApproval);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ForPostingCorrection(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForPostingCorrection);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult ApprovePosting(int id) // public IActionResult Create() / async Task<IActionResult>
        {
            if (this.userService.IsUserPosting(id, this.User.Id()))
            {
                var errText = UserPostingIdAndUserApprovePostingIdCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id, errText = errText });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.Finished);

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
