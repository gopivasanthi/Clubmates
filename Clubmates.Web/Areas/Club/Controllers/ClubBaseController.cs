using Clubmates.Web.Areas.Club.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Clubmates.Web.Areas.Club.Controllers
{
    [Area("Club")]
    [Route("Club/[controller]/[action]")]
    public class ClubBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["Area"] = "Club";
            ViewData["Layout"] = "~/Areas/Club/Views/Shared/_ClubLayout.cshtml";
            base.OnActionExecuting(context);
        }
    }
}
