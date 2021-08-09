namespace BankOrders.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BankOrders.Data.Models;

    public class Template : BaseDocument
    {
        public Template()
        {
            //this.Id = Guid.NewGuid().ToString();
            this.Details = new HashSet<Detail>();
            this.TimesUsed = 0;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public int TimesUsed { get; set; }
    }
}
