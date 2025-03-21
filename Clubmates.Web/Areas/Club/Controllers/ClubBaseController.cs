using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clubmates.Web.Areas.Club.Controllers
{
    [Area("Club")]
    [Route("Club/[controller]/[action]")]
    public class ClubBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["Area"] = "Club";
            ViewData["Layout"] = "~/Views/Shared/_ClubLayout.cshtml";
            base.OnActionExecuting(context);
        }
    }
}
