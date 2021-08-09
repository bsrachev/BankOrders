namespace BankOrders.Services.Details
{
    using System.Collections.Generic;

    public interface IDetailService
    {
        ICollection<DetailsServiceModel> GetDetails(int orderId);
    }
}
