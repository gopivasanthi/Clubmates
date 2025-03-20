using Clubmates.Web.AppDbContext;
using Clubmates.Web.Models;
using Clubmates.Web.Models.AccountViewModel;
using Clubmates.Web.Models.AdminViewModel;
using Clubmates.Web.Models.ClubsViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Security.Claims;

namespace Clubmates.Web.Controllers
{
    [Authorize(Policy = "MustbeAGuest")]
    public class ClubsController(
        AppIdentityDbContext dbContext,
        UserManager<ClubmatesUser> userManager) : Controller
    {
        private readonly AppIdentityDbContext _dbContext = dbContext;
        private readonly UserManager<ClubmatesUser> _userManager = userManager;
        public async Task<IActionResult> Index()
        {
            var listOfClubs = await _dbContext
                                        .Clubs
                                        .Include(x => x.ClubManager)
                                        .ToListAsync();

            var listOfClubsViewModel = listOfClubs.Select(club => new CustomerClubViewModel
            {
                ClubId = club.ClubId,
                ClubName = club.ClubName,
                ClubDescription = club.ClubDescription,
                ClubCategory = club.ClubCategory,
                ClubType = club.ClubType,
                ClubRules = club.ClubRules,
                ClubManager = club.ClubManager?.Email,
                ClubContactNumber = club.ClubContactNumber,
                ClubEmail = club.ClubEmail,
                ClubLogo = club.ClubLogo,
                ClubBanner = club.ClubBanner,
                ClubBackground = club.ClubBackground
            }).ToList();

            return View(listOfClubsViewModel);
        }
        public async Task<IActionResult> ClubDetails(int clubId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var clubDetails = await _dbContext
                                        .Clubs
                                        .Include(x => x.ClubManager)
                                        .FirstOrDefaultAsync(x => x.ClubId == clubId);

            if (clubDetails == null)
            {
                return RedirectToAction("Index");
            }
            var clubDetailsViewModel = new CustomerClubViewModel()
            {
                ClubManager = clubDetails.ClubManager?.Email,
                ClubName = clubDetails.ClubName,
                ClubDescription = clubDetails.ClubDescription,
                ClubCategory = clubDetails.ClubCategory,
                ClubType = clubDetails.ClubType,
                ClubRules = clubDetails.ClubRules,
                ClubContactNumber = clubDetails.ClubContactNumber,
                ClubEmail = clubDetails.ClubEmail,
                ClubLogo = clubDetails.ClubLogo,
                ClubBanner = clubDetails.ClubBanner,
                ClubBackground = clubDetails.ClubBackground
            };
            return View(clubDetailsViewModel);
        }
        public IActionResult CreateClubForCustomer()
        {
            return View(new CustomerClubViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateClubForCustomer(
                    CustomerClubViewModel customerClubViewModel,
                    IFormFile clubLogo,
                    IFormFile clubBanner,
                    IFormFile clubBackground)
        {
            if (!ModelState.IsValid)
            {
                return View(customerClubViewModel);
            }
            //who is the logged in user
            var loggedInUserEmail = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            //get that user from database
            if (loggedInUserEmail == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Clubs/CreateClubForCustomer" });
            }
            var loggedInUser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            if (customerClubViewModel != null && loggedInUser != null)
            {
                Club club = new()
                {
                    ClubName = customerClubViewModel.ClubName,
                    ClubDescription = customerClubViewModel.ClubDescription,
                    ClubCategory = customerClubViewModel.ClubCategory,
                    ClubType = customerClubViewModel.ClubType,
                    ClubRules = customerClubViewModel.ClubRules,
                    ClubManager = loggedInUser,
                    ClubContactNumber = customerClubViewModel.ClubContactNumber,
                    ClubEmail = customerClubViewModel.ClubEmail
                };
                if (clubLogo != null)
                {
                    using var memoryStream = new MemoryStream();
                    await clubLogo.CopyToAsync(memoryStream);
                    club.ClubLogo = memoryStream.ToArray();
                }
                if (clubBanner != null)
                {
                    using var memoryStream = new MemoryStream();
                    await clubBanner.CopyToAsync(memoryStream);
                    club.ClubBanner = memoryStream.ToArray();
                }
                if (clubBackground != null)
                {
                    using var memoryStream = new MemoryStream();
                    await clubBackground.CopyToAsync(memoryStream);
                    club.ClubBackground = memoryStream.ToArray();
                }
                var createdClubEntity = _dbContext.Clubs.Add(club);
                await _dbContext.SaveChangesAsync();

                if (createdClubEntity != null)
                {
                    var createdClub = await _dbContext.Clubs.FindAsync(createdClubEntity.Entity.ClubId);
                    if (createdClub != null)
                    {
                        bool isClubRoleAvailable = false;
                        if (await _userManager.GetClaimsAsync(loggedInUser) != null)
                        {
                            var userClaims = await _userManager.GetClaimsAsync(loggedInUser);
                            foreach (var claim in userClaims)
                            {
                                if (claim.Value == Enum.GetName(ClubmatesRole.ClubUser))
                                {
                                    isClubRoleAvailable = true;
                                }
                            }
                            if (!isClubRoleAvailable)
                            {
                                await _userManager
                                            .AddClaimAsync(
                                                    loggedInUser,
                                                    new(ClaimTypes.Role, Enum.GetName(ClubmatesRole.ClubUser) ?? ""));
                            }
                        }
                        loggedInUser.ClubmatesRole = ClubmatesRole.ClubUser;
                        //save the user
                        await _userManager.UpdateAsync(loggedInUser);
                        _dbContext.ClubAccesses.Add(new ClubAccess
                        {
                            Club = createdClub,
                            ClubmatesUser = loggedInUser,
                            ClubAccessRole = ClubAccessRole.ClubManager
                        });
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(customerClubViewModel);
        }
    }
}
