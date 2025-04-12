
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositories;
using Ecom.infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace Ecom.infrastructure
{
    public static class infrastructureRegisteration
    {

        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //apply unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //apply email service
            services.AddScoped<IEmailService, EmailService>();

            //apply GeneratToken
            services.AddScoped<IGenerateToken, GenerateToken>();

            //apply Redis connected
            services.AddSingleton<IConnectionMultiplexer>(
                i =>
                {
                    var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
                    return ConnectionMultiplexer.Connect(config);
                }
                );
            //add image service
            services.AddSingleton<IImageManagementService, ImageManagementService>();
            services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                    );

            //apply dbcontext
            services.AddDbContext<AppDbContext>(options =>
                                  options.UseSqlServer
                            (configuration.GetConnectionString("Default")));

            //add Identity User
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(cookieOptions =>
           {
               cookieOptions.Cookie.Name = "Token";
               cookieOptions.Events.OnRedirectToLogin = context =>
               {
                   context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                   return Task.CompletedTask;
               };
           }).AddJwtBearer(jwtOptions =>
           {
               jwtOptions.RequireHttpsMetadata = true;
               jwtOptions.SaveToken = true;

               jwtOptions.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(configuration["Token:Secret"])),

                   ValidateIssuer = true,
                   ValidIssuer = configuration["Token:Issuer"],

                   ValidateAudience = false,
                   ClockSkew = TimeSpan.Zero // Time between server and the client
               };

               jwtOptions.Events = new JwtBearerEvents
               {

                   /*
                    This lets you customize behavior when receiving the token.
                    Inside it:
                    OnMessageReceived = context => { ... }
                    This tells the app:
                   "Get the token from cookies instead of the default (header)."
                   context.Token = context.Request.Cookies["Token"];
                   Look for the token inside a cookie named "Token".
                    */
                   OnMessageReceived = context =>
                   {

                       context.Token = context.Request.Cookies["Token"];
                       return Task.CompletedTask;
                   }
               };
           });

            return services;

        }
    }
}
