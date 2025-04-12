
using Microsoft.AspNetCore.Identity;

namespace Ecom.Core.Entities
{
    public class AppUser :IdentityUser
    {
        public string  DisplayName{ get; set; }

        public Address Address{ get; set; }

    }
}
