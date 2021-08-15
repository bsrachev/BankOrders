namespace BankOrders.Services.Orders
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using BankOrders.Services.Users;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly IUserService userService;
        private readonly BankOrdersDbContext data;

        public OrderService(IUserService userService, BankOrdersDbContext data)
        {
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
                    string userCreateId = this.userService.GetUserIdByEmployeeNumber(searchModel.UserCreate);
                    
                    ordersQuery = ordersQuery.Where(x => x.UserCreateId == userCreateId);
                }

                if (searchModel.UserApprove != null)
                {
                    string userApproveId = this.userService.GetUserIdByEmployeeNumber(searchModel.UserApprove);
                    
                    ordersQuery = ordersQuery.Where(x => x.UserApproveId == userApproveId);
                }

                if (searchModel.UserPosting != null)
                {
                    string userPostingId = this.userService.GetUserIdByEmployeeNumber(searchModel.UserPosting);

                    ordersQuery = ordersQuery.Where(x => x.UserPostingId == userPostingId);
                }

                if (searchModel.UserApprovePosting != null)
                {
                    string userApprovePostingId = this.userService.GetUserIdByEmployeeNumber(searchModel.UserApprovePosting);

                    ordersQuery = ordersQuery.Where(x => x.UserApprovePostingId == userApprovePostingId);
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
                    UserCreateId = this.userService.GetUserInfo(c.UserCreateId).EmployeeNumber,
                    UserApproveId = this.userService.GetUserInfo(c.UserApproveId).EmployeeNumber,
                    UserPostingId = this.userService.GetUserInfo(c.UserPostingId).EmployeeNumber,
                    UserApprovePostingId = this.userService.GetUserInfo(c.UserApprovePostingId).EmployeeNumber,
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
                AccountingDate = DateTime.ParseExact(accountingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                System = (OrderSystem)Enum.Parse(typeof(OrderSystem), system, true),
                UserCreateId = userId
            };

            this.data.Orders.Add(order);

            this.data.SaveChanges();

            return order.Id;
        }
    }
}
