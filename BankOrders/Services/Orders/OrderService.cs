namespace BankOrders.Services.Orders
{
    using BankOrders.Data;
    using BankOrders.Data.Models.Enums;

    using System.Linq;

    public class OrderService : IOrderService
    {
        private readonly BankOrdersDbContext data;

        public OrderService(BankOrdersDbContext data)
            => this.data = data;


        /*public int RefNumber { get; set; }
        public DateTime? AccountingDate { get; set; }
        public OrderSystem System { get; set; }
        public string UserCreate { get; set; }
        public string UserApprove { get; set; }
        public string UserAccountant { get; set; }
        public string UserApproveAccounting { get; set; }
        public OrderStatus Status { get; set; }*/

        public OrderServiceModel GetOrderInfo(int id)
            => this.data
                .Orders
                .Where(c => c.Id == id)
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
