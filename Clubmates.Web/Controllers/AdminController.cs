using Clubmates.Web.Models.AccountViewModel;
using Clubmates.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Clubmates.Web.AppDbContext;
using Clubmates.Web.Models.AdminViewModel;

namespace Clubmates.Web.Controllers
{
    [Authorize(Policy = "MustbeASuperAdmin")]
    public class AdminController(UserManager<ClubmatesUser> userManager,
                                AppIdentityDbContext dbContext) : Controller
    {
        private readonly UserManager<ClubmatesUser> _userManager = userManager;
        private readonly AppIdentityDbContext _dbContext = dbContext;
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ManageUsers()
        {
            return View(await GetUsersToManageAsync());
        }

        private async Task<List<UserViewModel>> GetUsersToManageAsync()
        {
            var users = await _userManager.Users
                                         .Where(x => x.ClubmatesRole != ClubmatesRole.SuperAdmin)
                                         .ToListAsync();

            var listOfUserAccounts = new List<UserViewModel>();
            foreach (var user in users)
            {
                listOfUserAccounts.Add(new UserViewModel
                {
                    Email = user.Email,
                    Name = await GetNameForUser(user.Email ?? string.Empty),
                    Role = user.ClubmatesRole
                });
            }
            return listOfUserAccounts;
        }

        private async Task<string> GetNameForUser(string Email)
        {
            var accountuser = await _userManager.FindByEmailAsync(Email);
            if (accountuser != null)
            {
                var claims = await _userManager
                                        .GetClaimsAsync(accountuser);
                if (claims != null)
                {
                    return claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task<IActionResult> EditUser(string email)
        {
            var accountUser = await _userManager.FindByEmailAsync(email);
            if (accountUser != null)
            {
                UserViewModel userViewModel = new()
                {
                    Email = accountUser.Email,
                    Name = await GetNameForUser(accountUser.Email ?? string.Empty),
                    Role = accountUser.ClubmatesRole,
                    Roles = Enum.GetValues<ClubmatesRole>()
                                .Select(x => new SelectListItem
                                {
                                    Text = Enum.GetName(x),
                                    Value = x.ToString()
                                })
                };
                return View(userViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }
            else
            {
                if (!string.IsNullOrEmpty(userViewModel.Email))
                {
                    try
                    {
                        ClubmatesUser? clubmatesUser = await _userManager.FindByEmailAsync(userViewModel.Email);
                        if (clubmatesUser != null)
                        {
                            clubmatesUser.ClubmatesRole = userViewModel.Role;
                            var claims = await _userManager.GetClaimsAsync(clubmatesUser);
                            var removeResult = await _userManager.RemoveClaimsAsync(clubmatesUser, claims);
                            if (!removeResult.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Unable to update claim - removing existing claim failed!");
                                return View(userViewModel);
                            }
                            var claimsRequired = new List<Claim>
                            {
                                new (ClaimTypes.Name, userViewModel.Name ?? ""),
                                new (ClaimTypes.Role, Enum.GetName(userViewModel.Role) ?? ""),
                                new (ClaimTypes.NameIdentifier, clubmatesUser.Id),
                                new (ClaimTypes.Email, userViewModel.Email),
                            };
                            var addClaimResult = await _userManager.AddClaimsAsync(clubmatesUser, claimsRequired);
                            if (!addClaimResult.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Unable to update claim - adding new claim failed!");
                                return View(userViewModel);
                            }
                            var userUpdateResult = await _userManager.UpdateAsync(clubmatesUser);
                            if (!userUpdateResult.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Unable to update user - update method failed!");
                                return View(userViewModel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                        return View(userViewModel);
                    }
                }
            }
            return View("ManageUsers", await GetUsersToManageAsync());
        }

        public async Task<IActionResult> DeleteUser(string email)
        {
            var accountUser = await _userManager.FindByEmailAsync(email);
            if (accountUser != null)
            {
                await _userManager.DeleteAsync(accountUser);
                return View("ManageUsers", await GetUsersToManageAsync());
            }
            return NotFound();
        }
        public IActionResult CreateUser()
        {
            return View(new CreateUserViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel createUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserViewModel);
            }
            if (createUserViewModel != null
                && createUserViewModel.Email != null
                && !createUserViewModel.Email.Equals(createUserViewModel.ConfirmEmail))
            {
                ModelState.AddModelError(string.Empty, "Email and Confirm Email do not match!");
                return View(createUserViewModel);
            }
            if (createUserViewModel != null
                && createUserViewModel.Password != null
                && !createUserViewModel.Password.Equals(createUserViewModel.ConfirmPassword))
            {
                ModelState.AddModelError(string.Empty, "Password and Confirm Password do not match!");
                return View(createUserViewModel);
            }
            if (createUserViewModel != null)
            {
                ClubmatesUser clubmatesUser = new()
                {
                    Email = createUserViewModel.Email,
                    UserName = createUserViewModel.Email,
                    ClubmatesRole = createUserViewModel.Role
                };
                var createdUser = await _userManager
                                            .CreateAsync(clubmatesUser,
                                                        createUserViewModel?.Password ?? "password-1");

                if (!createdUser.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Password and Confirm Password do not match!");
                    return View(createUserViewModel);
                }
                if (createUserViewModel != null)
                {
                    var claimsRequired = new List<Claim>
                            {
                                new (ClaimTypes.Name, createUserViewModel?.Name ?? ""),
                                new (ClaimTypes.Role, Enum.GetName(createUserViewModel?.Role ?? 0) ?? ""),
                                new (ClaimTypes.NameIdentifier, clubmatesUser.Id),
                                new (ClaimTypes.Email, createUserViewModel?.Email ?? ""),
                            };
                    await _userManager.AddClaimsAsync(clubmatesUser, claimsRequired);
                    await _userManager.UpdateAsync(clubmatesUser);
                }
                return View("ManageUsers", await GetUsersToManageAsync());
            }
            return View(new CreateUserViewModel());
        }
        public async Task<IActionResult> ManageClubs()
        {
            var listOfClubs = await _dbContext
                                        .Clubs
                                        .Include(x => x.ClubManager)
                                        .ToListAsync();
            List<ClubViewModel> clubViewModels = listOfClubs.Select(x => new ClubViewModel
            {
                ClubId = x.ClubId,
                ClubName = x.ClubName,
                ClubDescription = x.ClubDescription,
                ClubCategory = x.ClubCategory,
                ClubType = x.ClubType,
                ClubRules = x.ClubRules,
                ClubManager = x.ClubManager?.Email,
                ClubContactNumber = x.ClubContactNumber,
                ClubEmail = x.ClubEmail
            }).ToList();

            return View(clubViewModels);
        }
        public IActionResult CreateClub()
        {
            return View(new ClubViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateClub(ClubViewModel clubViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(clubViewModel);
            }
            try
            {
                Club club = new()
                {
                    ClubName = clubViewModel.ClubName,
                    ClubDescription = clubViewModel.ClubDescription,
                    ClubCategory = clubViewModel.ClubCategory,
                    ClubType = clubViewModel.ClubType,
                    ClubRules = clubViewModel.ClubRules,
                    ClubContactNumber = clubViewModel.ClubContactNumber,
                    ClubEmail = clubViewModel.ClubEmail,
                    ClubManager = await _userManager.FindByEmailAsync(clubViewModel.ClubManager ?? "")
                };
                club.ClubManager = await _userManager.FindByEmailAsync(clubViewModel.ClubManager ?? "");
                _dbContext.Clubs.Add(club);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("ManageClubs");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(clubViewModel);
            }
        }
        public async Task<IActionResult> EditClub(int clubId)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("ManageClubs");
            }
            var club = await _dbContext
                                .Clubs
                                .Include(x => x.ClubManager)
                                .FirstOrDefaultAsync(x => x.ClubId == clubId);
            if (club != null)
            {
                ClubViewModel clubViewModel = new()
                {
                    ClubId = club.ClubId,
                    ClubName = club.ClubName,
                    ClubDescription = club.ClubDescription,
                    ClubCategory = club.ClubCategory,
                    ClubType = club.ClubType,
                    ClubRules = club.ClubRules,
                    ClubManager = club.ClubManager?.Email,
                    ClubContactNumber = club.ClubContactNumber,
                    ClubEmail = club.ClubEmail
                };
                return View(clubViewModel);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditClub(ClubViewModel clubViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(clubViewModel);
            }
            try
            {
                var club = await _dbContext.Clubs.FindAsync(clubViewModel.ClubId);
                if (club != null)
                {
                    club.ClubName = clubViewModel.ClubName;
                    club.ClubDescription = clubViewModel.ClubDescription;
                    club.ClubCategory = clubViewModel.ClubCategory;
                    club.ClubType = clubViewModel.ClubType;
                    club.ClubRules = clubViewModel.ClubRules;
                    club.ClubContactNumber = clubViewModel.ClubContactNumber;
                    club.ClubEmail = clubViewModel.ClubEmail;
                    club.ClubManager = _userManager.FindByEmailAsync(clubViewModel.ClubManager ?? "").Result;
                    _dbContext.Update(club);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("ManageClubs");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(clubViewModel);
            }
            return NotFound();
        }
        public async Task<IActionResult> DeleteClub(int clubId)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("ManageClubs");
            }
            var club = await _dbContext.Clubs.FindAsync(clubId);
            if(club != null)
            {
                var clubAccesses = await _dbContext
                                                .ClubAccesses
                                                .Where(x => x.Club == club)
                                                .ToListAsync();
                if (clubAccesses != null && clubAccesses.Count > 0)
                {
                    foreach (var clubAccess in clubAccesses)
                    {
                        _dbContext.Remove(clubAccess);
                    }
                }
                _dbContext.Remove(club);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("ManageClubs");
            }
            return NotFound();
        }
    }
}
