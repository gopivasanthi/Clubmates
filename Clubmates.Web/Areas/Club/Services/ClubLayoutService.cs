using Clubmates.Web.AppDbContext;
using Clubmates.Web.Areas.Club.Models;
using Clubmates.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Clubmates.Web.Areas.Club.Services
{
    public class ClubLayoutService(AppIdentityDbContext dbContext,
                                    UserManager<ClubmatesUser> userManager): IClubLayoutService
    {
        private readonly AppIdentityDbContext _dbContext = dbContext;
        private readonly UserManager<ClubmatesUser> _userManager = userManager;
        public async Task<ClubLayout> PopulateClubLayout(string loggedInUserEmail, int clubId)
        {
            var club = await _dbContext
                        .Clubs
                        .Include(x => x.ClubManager)
                        .FirstOrDefaultAsync(x => x.ClubId == clubId);

            var clubAccess = await _dbContext
                                      .ClubAccesses
                                      .Include(x => x.Club)
                                      .Include(x => x.ClubmatesUser)
                                      .Where(x => x.ClubmatesUser != null && x.ClubmatesUser.Email == loggedInUserEmail)
                                      .Where(x => x.Club != null && x.Club.ClubId == clubId)
                                      .FirstOrDefaultAsync();

            if (club != null)
            {
                ClubLayout clubLayout = new()
                {
                    MainMenus = DisplayMainMenu(clubAccess, clubId),
                    Logo = DisplayLogo(club?.ClubLogo)
                };
                return clubLayout;
            }
            return new ClubLayout();
        }
        public async Task<bool> ValidateClub(int? clubId)
        {
            var club = await _dbContext
                    .Clubs
                    .Include(x => x.ClubManager)
                    .FirstOrDefaultAsync(x => x.ClubId == clubId);
            return club != null;
        }
        public async Task<bool> ValidateClubUser(string? loggedInUserEmail)
        {
            var clubUser =  await _userManager.FindByEmailAsync(loggedInUserEmail ?? "");
            return clubUser != null;
        }
        private static List<MainMenu> DisplayMainMenu(ClubAccess? clubAccess, int? clubId)
        {
            if (clubAccess != null)
            {
                var mainMenuItems = new List<MainMenu>();
                switch (clubAccess.ClubAccessRole)
                {
                    case ClubAccessRole.ClubManager:
                        {
                            mainMenuItems.Add(new MainMenu
                            {
                                MenuArea = "Club",
                                MenuController = "Home",
                                MenuAction = "Index",
                                MenuTitle = "Club Details",
                                ClubId = clubId,
                            });
                            mainMenuItems.Add(new MainMenu
                            {
                                MenuArea = "Club",
                                MenuController = "ManageClub",
                                MenuAction = "Index",
                                MenuTitle = "Manager Club",
                                ClubId = clubId,
                            });
                            break;
                        }
                    case ClubAccessRole.ClubMember:
                        {
                            mainMenuItems.Add(new MainMenu
                            {
                                MenuArea = "Club",
                                MenuController = "Home",
                                MenuAction = "Index",
                                MenuTitle = "Club Details",
                                ClubId = clubId,
                            });
                            mainMenuItems.Add(new MainMenu
                            {
                                MenuArea = "Club",
                                MenuController = "Events",
                                MenuAction = "Index",
                                MenuTitle = "Events",
                                ClubId = clubId,
                            });
                            break;
                        }
                    case ClubAccessRole.ClubAdmin:
                        {
                            mainMenuItems.Add(new MainMenu
                            {
                                MenuArea = "Club",
                                MenuController = "Home",
                                MenuAction = "Index",
                                MenuTitle = "Club Details",
                                ClubId = clubId,
                            });
                            break;
                        }
                }
                return mainMenuItems;
            }
            return [];
        }

        private static string DisplayLogo(byte[]? clubLogo)
        {
            if (clubLogo != null)
            {
                var base64 = Convert.ToBase64String(clubLogo);
                var imgSrc = string.Format("data:image/png;base64,{0}", base64);
                return imgSrc;
            }
            return string.Empty;
        }
    }
}
