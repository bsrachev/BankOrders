namespace BankOrders.Models.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class OrderPostingNumberFormModel
    {
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Posting Number")]
        public int PostingNumber { get; set; }
    }
}
