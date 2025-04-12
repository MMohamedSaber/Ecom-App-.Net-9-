
using Ecom.Core.DTOs;

namespace Ecom.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDTO emailDTO);
    }
}
