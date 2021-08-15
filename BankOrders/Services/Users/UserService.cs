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

        public string GetUserIdByEmployeeNumber(string employeeNumber)
            => this.data
                .Users
                .Where(u => u.EmployeeNumber == employeeNumber)
                .FirstOrDefault()
                .Id;

        public UserServiceModel GetUserInfo(string userId)
            => this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(o => new UserServiceModel
                {
                    Id = userId,
                    FullName = o.FullName,
                    Email = o.Email,
                    EmployeeNumber = o.EmployeeNumber
                })
                .FirstOrDefault();

        public bool IsOrderUserCreate(int orderId, string userId)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserCreateId == userId);

        public bool IsTemplateUserCreate(int templateId, string userId)
            => this.data
                .Orders
                .Any(o => o.Id == templateId && o.UserCreateId == userId);

        public bool IsUserApprove(int orderId, string userId)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserApproveId == userId);

        public bool IsUserPosting(int orderId, string userId)
            => this.data
                .Orders
                .Any(o => o.Id == orderId && o.UserPostingId == userId);
    }
}
