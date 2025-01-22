using Microsoft.AspNetCore.Identity;

namespace Mocab.Web.Services.Data.Entities
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
