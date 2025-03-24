using Microsoft.AspNetCore.Mvc;

namespace Clubmates.Web.Areas.Club.Controllers
{
    public class ManageClubController : ClubBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
