using Clubmates.Web.AppDbContext;
using Clubmates.Web.Areas.Club.Models.ViewModels;
using Clubmates.Web.Areas.Club.Services;
using Clubmates.Web.Models;
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
        public async Task<IActionResult> ManageClubMembers(int? clubId)
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
            List<ClubMembersViewModel> clubMembers = [];
            return View(clubMembers);
        }
        public async Task<IActionResult> InviteMemberToClub(int? clubId)
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
            ClubMembersViewModel clubMember = new ();
            return View(clubMember);
        }
        public async Task<IActionResult> InviteMemberToClub(ClubMembersViewModel clubMembersViewModel)
        {
            if (!ModelState.IsValid)
                return View(clubMembersViewModel);
            var loggedInUserEmail = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (loggedInUserEmail == null)
                return Redirect("/Account/Login");
            if (await _clubLayoutService.ValidateClubUser(loggedInUserEmail))
                return Redirect("/Account/Login");
            var clubLayout = await _clubLayoutService.PopulateClubLayout(loggedInUserEmail, clubMembersViewModel.ClubId ?? 0);
            ViewBag.MainMenuItems = clubLayout.MainMenus;
            ViewBag.ImgSrc = clubLayout.Logo;
            ViewBag.ClubName = clubLayout.ClubName;

            //add logic for inviting member to the club

            return View("InviteSucess", clubMembersViewModel);
        }
        public async Task<IActionResult> RemoveUser(int? clubId, int? memberId)
        {
            if (!ModelState.IsValid)
                return Redirect("/Club/ManageClub/ManageClubMembers");
            var loggedInUserEmail = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (loggedInUserEmail == null)
                return Redirect("/Account/Login");
            if (await _clubLayoutService.ValidateClubUser(loggedInUserEmail))
                return Redirect("/Account/Login");
            var clubLayout = await _clubLayoutService.PopulateClubLayout(loggedInUserEmail, clubId ?? 0);
            ViewBag.MainMenuItems = clubLayout.MainMenus;
            ViewBag.ImgSrc = clubLayout.Logo;
            ViewBag.ClubName = clubLayout.ClubName;

            //logic for removing the user from the club

            return Redirect("/Club/ManageClub/ManageClubMembers");
        }
        public async Task<IActionResult> ManageClubEvents(int? clubId)
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
        public async Task<IActionResult> ManageClubPolls(int? clubId)
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
