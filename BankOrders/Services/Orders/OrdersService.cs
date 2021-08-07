namespace BankOrders.Services
{
    using BankOrders.Data;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Models.Orders;
    using BankOrders.Services.Orders;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OrdersService : IOrdersService
    {
        private readonly BankOrdersDbContext data;

        public OrdersService(BankOrdersDbContext data)
            => this.data = data;

        public ICollection<string> ValidateOrder(CreateOrderFormModel model)
        {
            var errors = new List<string>();

            if (model.System != "Internal" && model.System != "External")
            {
                errors.Add($"Invalid payment system.");
            }

            DateTime temp;
            if (!DateTime.TryParse(model.AccountingDate, out temp)) // TODO
            {
                errors.Add($"Invalid date.");
            }

            return errors;
        }

        /*public int RefNumber { get; set; }
        public DateTime? AccountingDate { get; set; }
        public OrderSystem System { get; set; }
        public string UserCreate { get; set; }
        public string UserApprove { get; set; }
        public string UserAccountant { get; set; }
        public string UserApproveAccounting { get; set; }
        public OrderStatus Status { get; set; }*/

        public OrderDetailsServiceModel Details(int id)
            => this.data
                .Orders
                .Where(c => c.Id == id)
                .Select(c => new OrderDetailsServiceModel
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

        public bool IsUserCreate(int orderId, string userId)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserCreate == userId);

        public bool IsUserApprove(int orderId, string userId)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserApprove == userId);

        public bool IsUserAccountant(int orderId, string userId)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserAccountant == userId);

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
