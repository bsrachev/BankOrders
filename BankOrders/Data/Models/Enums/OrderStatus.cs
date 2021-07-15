namespace BankOrders.Data.Models.Enums
{
    public enum OrderStatus
    {
        Draft = 0,
        ForApproval = 1,
        ForAccounting = 2,
        ForAccountingApproval = 3,
        Finished = 4,
    }
}
