using Microsoft.AspNetCore.Identity;

namespace Clubmates.Web.Models
{
    public class ClubmatesUser : IdentityUser
    {
        public ClubmatesRole ClubmatesRole { get; set; }
        public ClubmatesProficiency ClubmatesProficiency { get; set; }
        public string? AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
        public string? AddressLine3 { get; set; } = string.Empty;
        public string? AddressLine4 { get; set; } = string.Empty;

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
    public enum ClubmatesProficiency
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }
}
