﻿namespace BankOrders.Services.Details
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Data.Models.Enums;
    using BankOrders.Services.Orders;
    using System.Collections.Generic;
    using System.Linq;

    public class DetailService : IDetailService
    {
        private readonly BankOrdersDbContext data;

        public DetailService(BankOrdersDbContext data)
            => this.data = data;

        public void AddDetail(string account, AccountType accountType, int branch, int costCenter, int currencyId, int orderOrTemplateRefNum, int project, string reason, decimal sum, decimal sumBGN)
        {
            var detail = new Detail
            {
                Account = account,
                AccountType = accountType,
                Branch = branch,
                CostCenter = costCenter,
                CurrencyId = currencyId,
                OrderOrTemplateRefNum = orderOrTemplateRefNum,
                Project = project,
                Reason = reason,
                Sum = sum,
                SumBGN = sumBGN
            };

            this.data.Details.Add(detail);
            this.data.SaveChanges();
        }

        public void EditDetail(int detailId, string account, AccountType accountType, int branch, int costCenter, int currencyId, int project, string reason, decimal sum, decimal sumBGN)
        {
            var detail = this.data.Details.Find(detailId);

            detail.Account = account;
            detail.AccountType = accountType;
            detail.Branch = branch;
            detail.CostCenter = costCenter;
            detail.CurrencyId = currencyId;
            detail.Project = project;
            detail.Reason = reason;
            detail.Sum = sum;
            detail.SumBGN = sumBGN;
            
            this.data.SaveChanges();
        }

        public void DeleteDetail(int detailId)
        {
            var detail = this.data.Details.Find(detailId);

            this.data.Remove(detail);

            this.data.SaveChanges();
        }

        public IEnumerable<DetailsServiceModel> GetDetails(int refNum)
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
                    CurrencyId = c.CurrencyId,
                    Project = c.Project,
                    Reason = c.Reason,
                    Sum = c.Sum,
                    SumBGN = c.SumBGN
                })
                .ToList();

        public void CopyFromTemplate(int orderId, int templateId)
        {
            var order = this.data.Orders.Find(orderId);

            var template = this.data.Templates.Find(templateId);

            foreach (var detail in GetDetails(template.RefNumber))
            {
                AddDetail(detail.Account,
                          detail.AccountType,
                          detail.Branch,
                          detail.CostCenter,
                          detail.CurrencyId,
                          order.RefNumber,
                          detail.Project,
                          detail.Reason,
                          detail.Sum,
                          detail.SumBGN);
            }
        }
    }
}
