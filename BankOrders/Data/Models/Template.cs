namespace BankOrders.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PaymentOrders.Data.Models;

    public class Template : BaseDocument
    {
        public Template(string name)
        {
            //this.Id = Guid.NewGuid().ToString();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.TimesUsed = 0;
            this.Name = name;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public int TimesUsed { get; set; }
    }
}
