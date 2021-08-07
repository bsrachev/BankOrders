namespace BankOrders.Data.Models.Enums
{
    public enum OrderStatus
    {
        Draft = 0,
        ForApproval = 1,
        ForPosting = 2,
        ForPostingApproval = 3,
        Finished = 4,
        ForCorrection = 5,
        ForPostingCorrection = 6,
        Canceled = 7
    }
}
