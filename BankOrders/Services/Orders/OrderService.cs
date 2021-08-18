namespace BankOrders.Services.Orders
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Details;
    using BankOrders.Models.Orders;
    using BankOrders.Services.Currencies;
    using BankOrders.Services.Details;
    using BankOrders.Services.Templates;
    using BankOrders.Services.Users;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly ICurrencyService currencyService;
        private readonly ITemplateService templateService;
        private readonly IDetailService detailService;
        private readonly IUserService userService;
        private readonly BankOrdersDbContext data;

        public OrderService(
            IUserService userService,
            ICurrencyService currencyService,
            ITemplateService templateService,
            IDetailService detailService,
            BankOrdersDbContext data)
        {
            this.currencyService = currencyService;
            this.templateService = templateService;
            this.detailService = detailService;
            this.userService = userService;
            this.data = data;
        }

        public IEnumerable<OrderServiceModel> GetAllOrders(OrderSearchFormModel searchModel = null)
        {
            var ordersQuery = this.data.Orders.AsQueryable();

            if (searchModel != null)
            {
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
                    var accDateFrom = DateTime.ParseExact(searchModel.AccountingDateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    ordersQuery = ordersQuery.Where(x => x.AccountingDate >= accDateFrom);
                }

                if (searchModel.AccountingDateTo != null)
                {
                    var accDateTo = DateTime.ParseExact(searchModel.AccountingDateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    ordersQuery = ordersQuery.Where(x => x.AccountingDate <= accDateTo);
                }

                if (searchModel.UserCreate != null)
                {                    
                    ordersQuery = ordersQuery.Where(x => x.UserCreateId == searchModel.UserCreate);
                }

                if (searchModel.UserApprove != null)
                {                    
                    ordersQuery = ordersQuery.Where(x => x.UserApproveId == searchModel.UserApprove);
                }

                if (searchModel.UserPosting != null)
                {
                    ordersQuery = ordersQuery.Where(x => x.UserPostingId == searchModel.UserPosting);
                }

                if (searchModel.UserApprovePosting != null)
                {
                    ordersQuery = ordersQuery.Where(x => x.UserApprovePostingId == searchModel.UserApprovePosting);
                }

                if (searchModel.System != null)
                {
                    ordersQuery = ordersQuery.Where(x => x.System == (OrderSystem)Enum.Parse(typeof(OrderSystem), searchModel.System));
                }

                if (searchModel.PostingNumber != null)
                {
                    ordersQuery = ordersQuery.Where(x => x.PostingNumber == int.Parse(searchModel.PostingNumber));
                }
            }

            return ordersQuery
                .OrderByDescending(c => c.RefNumber)
                .Select(c => new OrderServiceModel
                {
                    Id = c.Id,
                    RefNumber = c.RefNumber,
                    AccountingDate = c.AccountingDate,
                    System = c.System,
                    UserCreateId = c.UserCreateId,
                    UserApproveId = c.UserApproveId,
                    UserPostingId = c.UserPostingId,
                    UserApprovePostingId = c.UserApprovePostingId,
                    PostingNumber = c.PostingNumber,
                    Status = c.Status
                })
                .ToList();
        }

        public OrderServiceModel GetOrderInfo(int orderId)
            => this.data
                .Orders
                .Where(c => c.Id == orderId)
                .Select(c => new OrderServiceModel
                {
                    Id = c.Id,
                    RefNumber = c.RefNumber,
                    AccountingDate = c.AccountingDate,
                    System = c.System,
                    UserCreateId = c.UserCreateId,
                    UserApproveId = c.UserApproveId,
                    UserPostingId = c.UserPostingId,
                    UserApprovePostingId = c.UserApprovePostingId,
                    Status = c.Status,
                    PostingNumber = c.PostingNumber
                })
                .FirstOrDefault();

        public bool ChangeStatus(int orderId, string userId, OrderStatus status)
        {
            var orderData = this.data.Orders.Find(orderId);

            orderData.Status = status;

            switch (status)
            {
                case OrderStatus.ForPosting: 
                    orderData.UserApproveId = userId;
                    break;
                case OrderStatus.ForPostingApproval:
                    orderData.UserPostingId = userId;
                    break;
                case OrderStatus.Finished:
                    orderData.UserApprovePostingId = userId;
                    break;
                case OrderStatus.ForPostingCorrection:
                    orderData.UserPostingId = null;
                    break;
            }

            data.SaveChanges();

            return true;
        }

        public int Create(string accountingDate, string system, string userId)
        {
            var order = new Order
            {
                AccountingDate = DateTime.ParseExact(accountingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                System = (OrderSystem)Enum.Parse(typeof(OrderSystem), system, true),
                UserCreateId = userId
            };

            this.data.Orders.Add(order);

            this.data.SaveChanges();

            return order.Id;
        }

        public void AddPostingNumber(int orderId, int postingNumber)
        {
            this.data.Orders.Find(orderId).PostingNumber = postingNumber;

            this.data.SaveChanges();
        }

        public void CancelOrder(int orderId)
        {
            this.data.Orders.Find(orderId).Status = OrderStatus.Canceled;

            this.data.SaveChanges();
        }

        public OrderDetailListingViewModel GetOrderWithEveryDetail(int orderId, int? editDetailId)
        {
            var query = new OrderDetailListingViewModel();

            var order = GetOrderInfo(orderId);

            var ordersDetailsQuery = this.detailService.GetDetails(order.RefNumber);

            var ordersDetailsList = new List<DetailFormModel>();

            foreach (var od in ordersDetailsQuery)
            {
                ordersDetailsList.Add(new DetailFormModel
                {
                    Account = od.Account,
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
            query.PostingNumber = order.PostingNumber;
            query.Details = ordersDetailsList;
            query.Currencies = this.currencyService.GetCurrencies();
            query.Templates = this.templateService.GetAllTemplatesBySystem(query.System);

            return query;
        }
    }
}
