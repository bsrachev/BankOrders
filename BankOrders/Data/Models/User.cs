namespace BankOrders.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public string EmployeeNumber { get; set; }
    }
}
