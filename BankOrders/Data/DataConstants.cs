namespace BankOrders.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FullNameMinLength = 10;
            public const int FullNameMaxLength = 50;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Errors
        {
            public const string NoDetailsError = "You cannot sent for approval an order with no details.";
            public const string UserCreateAndUserApproveCannotBeTheSameError = "You cannot appove an order that you have created.";
            public const string UserApproveAndUserAccountantCannotBeTheSameError = "You cannot do the posting for an order that you have approved.";
            public const string UserAccountantAndUserApproveAccountingCannotBeTheSameError = "You cannot approve the posting for an order that you have posted.";
        }
    }
}