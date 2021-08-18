namespace BankOrders.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Services.Orders;
    using BankOrders.Models.Orders;
    using BankOrders.Infrastructure;
    using BankOrders.Models.Details;
    using BankOrders.Services.Users;
    using BankOrders.Services.Details;
    using BankOrders.Services.Templates;
    using BankOrders.Services.Currencies;
    using BankOrders.Services.Email;
    using BankOrders.Services.Email.Models;

    using System;
    using System.Linq;

    using static Data.DataConstants.ErrorMessages;
    using static Data.DataConstants.SuccessMessages;
    using static WebConstants;

    public class OrdersController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly ITemplateService templateService;
        private readonly IOrderService orderService;
        private readonly IDetailService detailService;
        private readonly IUserService userService;
        private readonly IEmailService emailService;

        public OrdersController(
            ICurrencyService currencyService,
            ITemplateService templateService,
            IOrderService orderService,
            IDetailService detailService,
            IUserService userService,
            IEmailService emailService)
        {
            this.currencyService = currencyService;
            this.templateService = templateService;
            this.orderService = orderService;
            this.detailService = detailService;
            this.userService = userService;
            this.emailService = emailService;
        }

        [Authorize]
        public IActionResult All([FromQuery] AllOrdersQueryModel query)
        {
            query.Orders = this.orderService.GetAllOrders();

            return this.View(query);
        }

        [HttpPost]
        [Authorize]
        public IActionResult All(OrderSearchFormModel searchModel)
        {
            var orders = this.orderService.GetAllOrders(searchModel);

            return this.View(new AllOrdersQueryModel { Orders = orders });
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
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
            var query = this.orderService.GetOrderWithEveryDetail(orderId, editDetailId);

            return View(query);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(DetailFormModel detailModel, int orderId, int? editDetailId)
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

        [Authorize]
        public IActionResult SendForApproval(int id)
        {
            var order = this.orderService.GetOrderInfo(id);

            var details = this.detailService.GetDetails(order.RefNumber);

            if (details.Count() == 0)
            {
                TempData[GlobalErrorKey] = NoDetailsError;

                return RedirectToAction(nameof(Details), new { orderId = id });
            }

            if (!this.detailService.IsDebitEqualToCredit(details))
            {
                TempData[GlobalErrorKey] = DebitAndCreditAreNotEqual;

                return RedirectToAction(nameof(Details), new { orderId = id });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForApproval);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Approve(int id)
        {
            var order = this.orderService.GetOrderInfo(id);

            var details = this.detailService.GetDetails(order.RefNumber);

            if (!this.detailService.IsDebitEqualToCredit(details))
            {
                TempData[GlobalErrorKey] = DebitAndCreditAreNotEqual;

                return RedirectToAction(nameof(Details), new { orderId = id });
            }

            if (this.userService.IsOrderUserCreate(id, this.User.Id()))
            {
                TempData[GlobalErrorKey] = UserCreateIdAndUserApproveIdCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForPosting);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult ForCorrection(int id)
        {
            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForCorrection);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult SendForPostingApproval(int id)
        {
            if (this.userService.IsUserApprove(id, this.User.Id()))
            {
                TempData[GlobalErrorKey] = UserApproveIdAndUserPostingIdCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id });
            }

            if (this.orderService.GetOrderInfo(id).PostingNumber == 0)
            {
                TempData[GlobalErrorKey] = NoPostingNumber;

                return RedirectToAction(nameof(Details), new { orderId = id });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForPostingApproval);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult ForPostingCorrection(int id)
        {
            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.ForPostingCorrection);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult ApprovePosting(int id)
        {
            if (this.userService.IsUserPosting(id, this.User.Id()))
            {
                TempData[GlobalErrorKey] = UserPostingIdAndUserApprovePostingIdCannotBeTheSameError;

                return RedirectToAction(nameof(Details), new { orderId = id });
            }

            var changeStatus = this.orderService.ChangeStatus(id, this.User.Id(), OrderStatus.Finished);

            if (!changeStatus)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult CopyDetailsFromTemplate(int orderId, int templateId)
        {
            if (templateId == 0)
            {
                TempData[GlobalErrorKey] = NoTemplates;
            }
            else
            {
                this.detailService.CopyFromTemplate(orderId, templateId);
                TempData[GlobalSuccessKey] = SuccessfullyCopiedDetails;
            }

            return this.Redirect($"/Orders/Details?orderId={@orderId}");
        }

        [Authorize]
        public IActionResult DeleteDetail(int orderId, int detailId)
        {
            this.detailService.DeleteDetail(detailId);

            return this.Redirect($"/Orders/Details?orderId={@orderId}");
        }

        [Authorize]
        public IActionResult AddPostingNumber(OrderPostingNumberFormModel postingNumberModel)
        {
            if (postingNumberModel.PostingNumber == 0)
            {
                TempData[GlobalErrorKey] = NoPostingNumber;

                return this.Redirect($"/Orders/Details?orderId={postingNumberModel.OrderId}");
            }

            this.orderService.AddPostingNumber(postingNumberModel.OrderId, postingNumberModel.PostingNumber);

            TempData[GlobalSuccessKey] = SuccessfullyAddedPostingNumber;

            return this.Redirect($"/Orders/Details?orderId={postingNumberModel.OrderId}");
        }

        [Authorize]
        public IActionResult CancelOrder(int orderId)
        {
            this.orderService.CancelOrder(orderId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult SendMail(EmailServiceModel emailServiceModel)
        {
            this.emailService.ForwardOrder(emailServiceModel.OrderId, this.User.Id(), emailServiceModel.RecepientId);

            TempData[GlobalSuccessKey] = SuccessfullyForwardedOrder;

            return this.Redirect($"/Orders/Details?orderId={@emailServiceModel.OrderId}");
        }
    }
}
