using AutoMapper;
using Ecom.Api.helper;
using Ecom.Core.DTOs;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Ecom.Api.Controllers
{

    public class AccountsController : BaseController
    {
        public AccountsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            var result=await _work.AuthRepository.RegisterAsync(registerDto);

            if (result!= "done")
            {
                return BadRequest(new ResponsApi(400,result)); 
            }
            return Ok(new ResponsApi(200,result));
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result=await _work.AuthRepository.LoginAsync(login);

            if (result.StartsWith("please"))
            {
                return BadRequest(new ResponsApi(400, result)); 
            }

            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Strict

            });

            return Ok(new ResponsApi(200, result));
        }

        [HttpPost("active-account")]
        public async Task<IActionResult> ActiveAccount(ActiveAccountDTO activeAccount)
        {
            var result = await _work.AuthRepository.ActiveAccount(activeAccount);
            return result ? Ok(new ResponsApi(200)) : BadRequest(new ResponsApi(400));
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> resetPassword(ResetPasswordDTO reset)
        {

            var result=await _work.AuthRepository.ResetPassword(reset);

            if (result == "Password Changed Successfuly")
            {
                return Ok(new ResponsApi(200,result));
            }

            return Ok(new ResponsApi(400, result));



        }

        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await _work.AuthRepository.SendEmailForForgetPassword(email);
            return result ? Ok(new ResponsApi(200)) : BadRequest(new ResponsApi(400));
        }


    }
}
