using Clubmates.Web.AppDbContext;
using Clubmates.Web.Areas.Club.Models;
using Clubmates.Web.Models;
using Clubmates.Web.Models.ClubsViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clubmates.Web.Areas.Club.Controllers
{
    public class HomeController(
                    AppIdentityDbContext dbContext,
                    UserManager<ClubmatesUser> userManager) : ClubBaseController
    {
        private readonly AppIdentityDbContext _dbContext = dbContext;
        private readonly UserManager<ClubmatesUser> _userManager = userManager;
        public async Task<IActionResult> Index(int? clubId = 0)
        {
            if (!ModelState.IsValid)
                return Redirect("/Clubs/Index");

            var club = await _dbContext
                                .Clubs
                                .Include(x => x.ClubManager)
                                .FirstOrDefaultAsync(x => x.ClubId == clubId);

            var loggedInUserEmail = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (loggedInUserEmail == null)
                return Redirect("/Account/Login");

            var clubuser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            if (clubuser == null)
                return Redirect("/Account/Login");

            var clubAccess = await _dbContext
                                      .ClubAccesses
                                      .Include(x => x.Club)
                                      .Include(x => x.ClubmatesUser)
                                      .Where(x => x.ClubmatesUser != null && x.ClubmatesUser.Email == loggedInUserEmail)
                                      .Where(x => x.Club != null && x.Club.ClubId == clubId)
                                      .FirstOrDefaultAsync();
            DisplayMainMenu(clubAccess, clubId);
            await DisplayLogo(clubId);
            var clubViewModel = new CustomerClubViewModel();
            if (club != null)
            {
                clubViewModel.ClubId = club.ClubId;
                clubViewModel.ClubName = club.ClubName;
                clubViewModel.ClubContactNumber = club.ClubContactNumber;
                clubViewModel.ClubManager = club.ClubManager?.Email;
                clubViewModel.ClubLogo = club.ClubLogo;
                clubViewModel.ClubBanner = club.ClubBanner;
                clubViewModel.ClubBackground = club.ClubBackground;
                clubViewModel.ClubCategory = club.ClubCategory;
                clubViewModel.ClubType = club.ClubType;
            }
            return View(clubViewModel);
        }
        private void DisplayMainMenu(ClubAccess? clubAccess, int? clubId)
        {
            if (clubAccess == null) { return; }
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
                ViewBag.MainMenuItems = mainMenuItems;
            }
        }
        private async Task DisplayLogo(int? clubId)
        {
            var logo = await _dbContext.Clubs.FindAsync(clubId);
            if (logo != null)
            {
                var base64 = Convert.ToBase64String(logo.ClubLogo ?? []);
                var imgSrc = string.Format("data:image/png;base64,{0}", base64);
                ViewBag.ImgSrc = imgSrc;
            }
        }
    }

}
