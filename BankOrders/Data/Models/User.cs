namespace BankOrders.Data.Models
{
    using static DataConstants.User;

    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        [RegularExpression(@"(BO\d{3})")] //TODO 6
        public string EmployeeNumber { get; set; }

        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }
    }
}
