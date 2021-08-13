namespace BankOrders.Services.Orders
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;

    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly BankOrdersDbContext data;

        public OrderService(BankOrdersDbContext data)
            => this.data = data;

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
