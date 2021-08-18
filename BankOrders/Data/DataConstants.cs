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
            public const string DebitAndCreditAreNotEqual = "The debit and credit sums must be equal.";
            public const string UserCreateIdAndUserApproveIdCannotBeTheSameError = "You cannot appove an order that you have created.";
            public const string UserApproveIdAndUserPostingIdCannotBeTheSameError = "You cannot do the posting for an order that you have approved.";
            public const string UserPostingIdAndUserApprovePostingIdCannotBeTheSameError = "You cannot approve the posting for an order that you have posted.";
            public const string NoPostingNumber = "The posting number cannot be 0 or empty.";
            public const string InvalidCurrencyCode = "The currency code is invalid.";
            public const string CurrencyCodeAlreadyExists = "The currency already exists.";
            public const string CannotDeleteEURorBGN = "You cannot delete this currency as it is a base one.";
            public const string NoTemplates = "There are no templates for this system.";
        }

        public class SuccessMessages
        {
            public const string SuccessfullyAddedCurrency = "Successfully added the currency.";
            public const string SuccessfullyDeletedCurrency = "Successfully deleted the currency.";
            public const string SuccessfullyCopiedDetails = "Successfully copied the details.";
            public const string SuccessfullyAddedPostingNumber = "Successfully added the posting number.";
            public const string SuccessfullyForwardedOrder = "Successfully forwarded the order.";
        }
    }
}