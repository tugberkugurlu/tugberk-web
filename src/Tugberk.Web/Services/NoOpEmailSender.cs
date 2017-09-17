using System.Threading.Tasks;

namespace Tugberk.Web.Services
{
    public class NoOpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message) =>
            Task.CompletedTask;
    }
}