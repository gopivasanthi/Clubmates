using Clubmates.Web.Models;
using Clubmates.Web.Models.AccountViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clubmates.Web.Controllers
{
    public class AccountController(UserManager<ClubmatesUser> userManager) : Controller
    {
        private readonly UserManager<ClubmatesUser> _userManager = userManager;

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            if (!registerViewModel.Password.Equals(registerViewModel.ConfirmPassword))
            {
                ModelState.AddModelError("Password", "Passwords do not match");
                return View(registerViewModel);
            }
            ClubmatesUser user = new ()
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                ClubmatesRole = ClubmatesRole.Guest
            };
            // Create the user
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerViewModel);
            }
            else
            {
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, registerViewModel.FullName),
                    new (ClaimTypes.Email, user.Email),
                    new (ClaimTypes.NameIdentifier, user.Id),
                    new (ClaimTypes.Role, "Guest")
                };
                var claimResult = await _userManager.AddClaimsAsync(user, claims);
                await _userManager.UpdateAsync(user);
                if(!claimResult.Succeeded)
                {
                    foreach (var error in claimResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(registerViewModel);
                }
            }
            return View("Success");

        }
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt, user not found");
                return View(loginViewModel);
            }
            var result = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt, password is incorrect");
                return View(loginViewModel);
            }
            else
            {
                var claims = user != null ? await _userManager.GetClaimsAsync(user) : null;
                if (claims != null)
                {
                    var scheme = IdentityConstants.ApplicationScheme;
                    var claimsIdentity = new ClaimsIdentity(claims, scheme);
                    var principal = new ClaimsPrincipal(claimsIdentity);
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                    };
                    await HttpContext.SignInAsync(scheme, principal, authenticationProperties);
                    return Redirect(ReturnUrl);
                }
            }

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Redirect("/Home/Index");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
