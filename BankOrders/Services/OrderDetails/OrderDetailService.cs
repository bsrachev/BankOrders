namespace BankOrders.Services.OrderDetails
{
    using BankOrders.Data;
    using BankOrders.Services.Orders;
    using System.Collections.Generic;
    using System.Linq;

    public class OrderDetailService : IOrderDetailService
    {
        private readonly BankOrdersDbContext data;

        public OrderDetailService(BankOrdersDbContext data)
            => this.data = data;

        public ICollection<OrderDetailsServiceModel> GetDetails(int orderId)
            => this.data
                .OrderDetails
                .Where(c => c.OrderOrTemplateRefNum == orderId)
                .Select(c => new OrderDetailsServiceModel
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
