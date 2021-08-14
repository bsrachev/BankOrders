namespace BankOrders.Services.Users
{
    using BankOrders.Data.Models.Enums;

    public class UserServiceModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string EmployeeNumber { get; set; }

        public string Email { get; set; }
    }
}
