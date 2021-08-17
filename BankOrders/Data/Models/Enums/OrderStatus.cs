namespace BankOrders.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum OrderStatus
    {
        [Display(Name = "Draft")]
        Draft = 0,

        [Display(Name = "For Approval")]
        ForApproval = 1,

        [Display(Name = "For Posting")]
        ForPosting = 2,

        [Display(Name = "For Posting Approval")]
        ForPostingApproval = 3,

        [Display(Name = "Finished")]
        Finished = 4,

        [Display(Name = "For Correction")]
        ForCorrection = 5,

        [Display(Name = "For Posting Correction")]
        ForPostingCorrection = 6,

        [Display(Name = "Canceled")]
        Canceled = 7
    }
}
