
using Microsoft.AspNetCore.Http;

namespace Ecom.Core.Services
{
    public interface IImageManagementService
    {
        Task<List<string>> AddImageAsync(IFormFileCollection files, string src);
        void DeleteImageAsync(string src);
    }
}
