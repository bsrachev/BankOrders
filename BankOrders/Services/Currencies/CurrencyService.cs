namespace BankOrders.Services.Currencies
{
    using BankOrders.Data;
    using BankOrders.Data.Models;
    using BankOrders.Services.Currencies.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    using static Data.DataConstants.ErrorMessages;
    using static WebConstants;

    public class CurrencyService : ICurrencyService
    {
        private readonly BankOrdersDbContext data;

        public object TempData { get; private set; }

        public CurrencyService(BankOrdersDbContext data)
            => this.data = data;

        public IEnumerable<CurrencyServiceModel> GetCurrencies()
            => this.data
                .Currencies
                .Select(c => new CurrencyServiceModel
                {
                    Id = c.Id,
                    Code = c.Code,
                    ExchangeRate = c.ExchangeRate
                })
                .ToList();

        public int Add(string code)
        {
            string siteContent = string.Empty;
            string url = "https://www.bnb.bg/Statistics/StExternalSector/StExchangeRates/StERForeignCurrencies/index.htm";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())  // Go query google
            using (Stream responseStream = response.GetResponseStream())               // Load the response stream
            using (StreamReader streamReader = new StreamReader(responseStream))       // Load the stream reader to read the response
            {
                siteContent = streamReader.ReadToEnd(); // Read the entire response and store it in the siteContent variable
            }

            char qm = Convert.ToChar("\u0022"); //unicode for quotation mark: "
            string pattern = @"<td class=" + qm + "center" + qm + ">(?<currencyCode>[A-Za-z]{3})<\\/td>\n" +
                              "<td class=" + qm + "right" + qm + ">(?<unit>10*)<\\/td>\n" +
                              "<td class=" + qm + "center" + qm + ">(?<rateForUnit>\\d+\\.\\d+)<\\/td>";

            MatchCollection matches = Regex.Matches(siteContent, pattern);
            var currencyInfo = matches.FirstOrDefault(c => c.Groups["currencyCode"].Value == code);
            if (currencyInfo == null || this.data.Currencies.Any(c => c.Code == code))
            {
                return 0;
            }
            else
            {
                var currencyExhangeRate = Convert.ToDecimal(currencyInfo.Groups["rateForUnit"].Value) / Convert.ToDecimal(currencyInfo.Groups["unit"].Value);
                
                var currency = new Currency
                {
                    Code = currencyInfo.Groups["currencyCode"].Value,
                    ExchangeRate = currencyExhangeRate
                };

                this.data.Currencies.Add(currency);

                this.data.SaveChanges();

                return currency.Id;
            }
        }

        public void Delete(int id)
        {
            var currency = this.data.Currencies.Find(id);

            this.data.Currencies.Remove(currency);

            this.data.SaveChanges();
        }

        public CurrencyServiceModel GetCurrencyInfo(int currencyId)
            => this.data
                .Currencies
                .Where(c => c.Id == currencyId)
                .Select(c => new CurrencyServiceModel
                {
                    Id = c.Id,
                    Code = c.Code,
                    ExchangeRate = c.ExchangeRate
                })
                .FirstOrDefault();
    }
}
