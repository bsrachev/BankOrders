namespace BankOrders.Services.Users
{
    using BankOrders.Data;
    using BankOrders.Infrastructure;

    using System.Linq;

    public class UserService : IUserService
    {
        private readonly BankOrdersDbContext data;

        public UserService(BankOrdersDbContext data)
            => this.data = data;


        public bool IsUserCreate(int orderId, string user)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserCreate == user);

        public bool IsUserApprove(int orderId, string user)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserApprove == user);

        public bool IsUserAccountant(int orderId, string user)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserAccountant == user);
    }
}
