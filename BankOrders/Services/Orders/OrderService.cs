namespace BankOrders.Services.Orders
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly BankOrdersDbContext data;

        public OrderService(BankOrdersDbContext data)
            => this.data = data;

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
            }

            return ordersQuery
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
                    UserCreate = c.UserCreate,
                    UserApprove = c.UserApprove,
                    UserAccountant = c.UserAccountant,
                    UserApproveAccounting = c.UserApproveAccounting,
                    Status = c.Status
                    //AccountingNumbers = c. TODO
                })
                .FirstOrDefault();

        public bool ChangeStatus(int orderId, string userId, OrderStatus status)
        {
            var orderData = this.data.Orders.Find(orderId);

            orderData.Status = status;

            switch (status)
            {

                case OrderStatus.ForPosting: 
                    orderData.UserApprove = userId;
                    break;
                case OrderStatus.ForPostingApproval:
                    orderData.UserAccountant = userId;
                    break;
                case OrderStatus.Finished:
                    orderData.UserApproveAccounting = userId;
                    break;
                case OrderStatus.ForPostingCorrection:
                    orderData.UserAccountant = null;
                    break;
            }

            data.SaveChanges();

            return true;
        }
    }
}
