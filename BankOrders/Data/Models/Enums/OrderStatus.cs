namespace BankOrders.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum OrderStatus
    {
        [Display(Name = "Draft")]
        Draft = 0,

        [Display(Name = "For approval")]
        ForApproval = 1,

        [Display(Name = "For posting")]
        ForPosting = 2,

        [Display(Name = "For posting approval")]
        ForPostingApproval = 3,

        [Display(Name = "Finished")]
        Finished = 4,

        [Display(Name = "For correction")]
        ForCorrection = 5,

        [Display(Name = "For posting correction")]
        ForPostingCorrection = 6,

        [Display(Name = "Canceled")]
        Canceled = 7
    }
}
