namespace BankOrders.Services.Users
{
    public interface IUserService
    {
        public bool IsUserCreateId(int orderId, string user);

        public bool IsUserApproveId(int orderId, string user);

        public bool IsUserPostingId(int orderId, string user);
    }
}
