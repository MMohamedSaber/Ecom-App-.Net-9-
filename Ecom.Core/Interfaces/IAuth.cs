
using Ecom.Core.DTOs;

namespace Ecom.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task SendEmail(string Email, string code, string componant, string subject, string message);
        Task<string> LoginAsync(LoginDTO loginDto);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
        Task<bool> ActiveAccount(ActiveAccountDTO accountDto);
    }
}
