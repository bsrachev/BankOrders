namespace BankOrders.Services.Details
{
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using System.Collections.Generic;

    public interface IDetailService
    {
        IEnumerable<DetailsServiceModel> GetDetails(int orderId);

        void AddDetail(string account,
                       AccountType accountType,
                       int branch,
                       int costCenter,
                       int currencyId,
                       int orderOrTemplateRefNum,
                       int project,
                       string reason,
                       decimal sum,
                       decimal sumBGN);

        void EditDetail(int detailId, 
                        string account, 
                        AccountType accountType, 
                        int branch, 
                        int costCenter, 
                        int currencyId, 
                        int project, 
                        string reason, 
                        decimal sum, 
                        decimal sumBGN);

        void CopyFromTemplate(int orderId, int templateId);

        void DeleteDetail(int detailId);
    }
}
