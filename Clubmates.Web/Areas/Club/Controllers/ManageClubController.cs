using Clubmates.Web.AppDbContext;
using Clubmates.Web.Areas.Club.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clubmates.Web.Areas.Club.Controllers
{
    public class ManageClubController(IClubLayoutService clubLayoutService) : ClubBaseController
    {
        private readonly IClubLayoutService _clubLayoutService = clubLayoutService;
        public async Task<IActionResult> Index(int? clubId)
        {
            if (!ModelState.IsValid)
                return Redirect("/Clubs/Index");

            var loggedInUserEmail = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (loggedInUserEmail == null)
                return Redirect("/Account/Login");
            if (await _clubLayoutService.ValidateClubUser(loggedInUserEmail))
                return Redirect("/Account/Login");

            var clubLayout = await _clubLayoutService.PopulateClubLayout(loggedInUserEmail, clubId ?? 0);

            ViewBag.MainMenuItems = clubLayout.MainMenus;
            ViewBag.ImgSrc = clubLayout.Logo;
            ViewBag.ClubName = clubLayout.ClubName;
            return View();
        }
    }
}
