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


        public bool IsUserCreateId(int orderId, string user)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserCreateId == user);

        public bool IsUserApproveId(int orderId, string user)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserApproveId == user);

        public bool IsUserPostingId(int orderId, string user)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserPostingId == user);
    }
}
