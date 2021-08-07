namespace BankOrders.Services.Users
{
    public interface IUserService
    {
        public bool IsUserCreate(int orderId, string user);

        public bool IsUserApprove(int orderId, string user);

        public bool IsUserAccountant(int orderId, string user);
    }
}
