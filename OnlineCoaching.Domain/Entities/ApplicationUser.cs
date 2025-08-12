using Microsoft.AspNetCore.Identity;

namespace OnlineCoaching.Domain.Entities
{
    //[Index(nameof(Email), IsUnique = true)]
    //[Index(nameof(UserName), IsUnique = true)]
    public class ApplicationUser : IdentityUser
    {
    
    }
}
