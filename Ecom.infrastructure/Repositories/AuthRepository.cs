
using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Ecom.infrastructure.Repositories
{
    public class AuthRepository:IAuth
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IGenerateToken _generateToken;

        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _generateToken = generateToken;
        }


        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
                return null;


            if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "this UserName is already Registerd";
            }
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "this Email is already Registerd";
            }

            AppUser appUser = new AppUser()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                DisplayName=registerDTO.DisplayName,
            };

            var result=await _userManager.CreateAsync(appUser,registerDTO.Password);
            
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }

            // send email to 
            var token=await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
          await  SendEmail(appUser.Email, token, "active-account", "ActiveMail", "Please Active your email by click on the button");


            return "done";


        }
    
        public async Task SendEmail(string Email,string code,string componant,string subject,string message)
        {
            var result = new EmailDTO(
                Email,
                "mohamedsabertamer1@gmail.com",
                subject,
                EmailStringBody.Send(Email, code, componant, message)
                );

           await _emailService.SendEmail(result);
        }
    
        public async Task<string > LoginAsync(LoginDTO loginDto)
        {

            var findUser=await _userManager.FindByEmailAsync(loginDto.Email);

            // is it have this email
            if (!findUser.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token, "Active", "ActiveMail", "Please Active your email by click on the button");
                return "Please confirm your email first ,we have send activate code to your email";
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                findUser, 
                loginDto.Password,
                true);

            if (result.Succeeded)
            {
                return _generateToken.GetAndCreateToken(findUser);
            }
            return "check your email or password something went wrong.";

        }

        public async Task<bool> SendEmailForForgetPassword(string email)
        {

            var findUser=await _userManager.FindByEmailAsync(email);

            if (findUser is  null)
            {
                return false;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "reset-password", "resetPassword", " click on the button to reset password");
            return true;
        }

        public async Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);

            if (findUser is null)
            {
                return null;
            }
            var decodedToken = Uri.UnescapeDataString(resetPasswordDTO.Token);

            var result =await _userManager.ResetPasswordAsync(findUser, decodedToken, resetPasswordDTO.Password);

            if (result.Succeeded)
            {
                return "Password Changed Successfuly";
            }

            return result.Errors.ToList()[0].Description;


        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDto)
        {

            var findUser = await _userManager.FindByEmailAsync(accountDto.Email);

            if (findUser is null)
                return false;

            var decodedToken = Uri.UnescapeDataString(accountDto.Token);
            var result=await _userManager.ConfirmEmailAsync(findUser, decodedToken);


            if (result.Succeeded)
                return true;


            // send email to ,if not active
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active-account", "ActiveMail", "Please Active your email by click on the button");

            return false;   


        }

    }
}
