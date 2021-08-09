namespace BankOrders.Services.Details
{
    using BankOrders.Data;
    using BankOrders.Services.Orders;
    using System.Collections.Generic;
    using System.Linq;

    public class DetailService : IDetailService
    {
        private readonly BankOrdersDbContext data;

        public DetailService(BankOrdersDbContext data)
            => this.data = data;

        public ICollection<DetailsServiceModel> GetDetails(int refNum)
            => this.data
                .Details
                .Where(c => c.OrderOrTemplateRefNum == refNum)
                .Select(c => new DetailsServiceModel
                {
                    Account = c.Account,
                    AccountingNumber = c.AccountingNumber,
                    AccountType = c.AccountType,
                    Branch = c.Branch,
                    CostCenter = c.CostCenter,
                    Currency = c.Currency,
                    Project = c.Project,
                    Reason = c.Reason,
                    Sum = c.Sum,
                    SumBGN = c.SumBGN
                })
                .ToList();
    }
}
