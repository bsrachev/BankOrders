namespace BankOrders.Services.Email
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);

        void ForwardOrder(int orderId, string senderId, string recepientId);
    }
} 
