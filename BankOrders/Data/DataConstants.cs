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

        public class ErrorMessages
        {
            public const string NoDetailsError = "You cannot sent an order for approval with no details.";
            public const string UserCreateIdAndUserApproveIdCannotBeTheSameError = "You cannot appove an order that you have created.";
            public const string UserApproveIdAndUserPostingIdCannotBeTheSameError = "You cannot do the posting for an order that you have approved.";
            public const string UserPostingIdAndUserApprovePostingIdCannotBeTheSameError = "You cannot approve the posting for an order that you have posted.";
            public const string InvalidCurrencyCode = "The currency has already been added or the currency code is invalid.";
            public const string CannotDeleteEURorBGN = "You cannot delete this currency as it is a base one.";
            public const string NoTemplates = "There are no templates for this system.";
        }

        public class SuccessMessages
        {
            public const string SuccessfullyAddedCurrency = "Successfully added the currency.";
            public const string SuccessfullyDeletedCurrency = "Successfully deleted the currency.";
            public const string SuccessfullyCopiedDetails = "Successfully copied the details.";
        }
    }
}