using System.Threading.Tasks;

namespace Tugberk.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}