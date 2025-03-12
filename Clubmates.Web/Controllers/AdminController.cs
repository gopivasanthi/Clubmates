using Clubmates.Web.Models.AccountViewModel;
using Clubmates.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clubmates.Web.Controllers
{
    [Authorize(Policy = "MustbeASuperAdmin")]
    public class AdminController(UserManager<ClubmatesUser> userManager) : Controller
    {
        private readonly UserManager<ClubmatesUser> _userManager = userManager;
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ManageUsers()
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
                    Role = Enum.GetName(user.ClubmatesRole)
                });
            }

            return View(listOfUserAccounts);
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
    }
}
