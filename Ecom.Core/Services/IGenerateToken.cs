
using Ecom.Core.Entities;

namespace Ecom.Core.Services
{
    public interface IGenerateToken
    {
        string GetAndCreateToken(AppUser appUser);

    }
}
