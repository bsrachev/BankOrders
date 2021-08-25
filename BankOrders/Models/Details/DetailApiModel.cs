namespace BankOrders.Models.Details
{
    public class DetailApiModel
    {
        public int Id { get; set; }

        public int Branch { get; set; }

        public int CostCenter { get; set; }

        public int Project { get; set; }

        public string Reason { get; set; }

        public string Account { get; set; }

        public string AccountTypeName { get; set; }

        public decimal Sum { get; set; }

        public string CurrencyName { get; set; }

        public decimal SumBGN { get; set; }
    }
}
