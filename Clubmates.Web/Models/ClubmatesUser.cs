using Microsoft.AspNetCore.Identity;

namespace Clubmates.Web.Models
{
    public class ClubmatesUser : IdentityUser
    {
        public ClubmatesRole Role { get; set; }
    }
    public enum ClubmatesRole
    {
        User,
        Guest,
        ClubMember,
        ClubManager,
        ClubAdmin,
        SuperAdmin
    }
}
