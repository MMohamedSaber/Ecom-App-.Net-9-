using Ecom.Api.helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.Api.Middleware
{
    public class MiddlewareExeptions
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow=TimeSpan.FromSeconds(30);

        public MiddlewareExeptions(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var respons = new ApiException((int)HttpStatusCode.TooManyRequests,
                        "Too Many Requests,Try Again later");

                    await context.Response.WriteAsJsonAsync(respons);


                }


               await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var respons = _environment.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                :
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message);
                var json=JsonSerializer.Serialize(respons);
                await context.Response.WriteAsync(json);


            }
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;

            var (timeStamp, count) = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timeStamp: dateNow, count: 0);
            });

            if (dateNow - timeStamp < _rateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }

                _memoryCache.Set(cacheKey, (timeStamp, count + 1), _rateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cacheKey, (timeStamp, count), _rateLimitWindow);
            }

            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";

        }
    }
}
