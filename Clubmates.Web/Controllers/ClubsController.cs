using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clubmates.Web.Controllers
{
    [Authorize(Policy = "MustbeAGuest")]
    public class ClubsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
