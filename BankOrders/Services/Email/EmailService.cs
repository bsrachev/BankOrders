namespace BankOrders.Services.Email
{
    using BankOrders.Services.Users;
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using MimeKit.Text;
    using System.Threading.Tasks;

    public class EmailService : IEmailService
    {
        private readonly IUserService userService;

        public EmailSenderOptions Options { get; set; }

        public EmailService(IOptions<EmailSenderOptions> options, IUserService userService)
        {
            this.Options = options.Value;
            this.userService = userService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(email, subject, message);
        }

        public Task Execute(string to, string subject, string message)
        {
            // create message
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(Options.Sender_EMail);
            if (!string.IsNullOrEmpty(Options.Sender_Name))
                email.Sender.Name = Options.Sender_Name;
            email.From.Add(email.Sender);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            // send email
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(Options.Host_Address, Options.Host_Port, Options.Host_SecureSocketOptions);
                smtp.Authenticate(Options.Host_Username, Options.Host_Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            return Task.FromResult(true);
        }

        public void ForwardOrder(int orderId, string senderId, string recepientId)
        {
            var sender = this.userService.GetUserInfo(senderId);
            var recepientMail = this.userService.GetUserInfo(recepientId).Email;
            var subject = "Forwarded order";
            var text = $"An order was forwarded to you by {sender.EmployeeNumber}. You can view it <a href=\"https://localhost:5001/Orders/Details?orderId={orderId}\">here</a>.<br/><br/>"
                       + "_____________<br/>"
                       + "<b>BankOrders</b>™<br/>"
                       + "Accounting services";

            SendEmailAsync(recepientMail, subject, text);
        }
    }
}
