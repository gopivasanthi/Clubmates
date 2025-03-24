using Clubmates.Web.Areas.Club.Models;

namespace Clubmates.Web.Areas.Club.Services
{
    public interface IClubLayoutService
    {
        public Task<ClubLayout> PopulateClubLayout(string loggedInUserEmail, int clubId);
        public Task<bool> ValidateClub(int? clubId);
        public Task<bool> ValidateClubUser(string? loggedInUserEmail);
    }
}
