using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Clubmates.Web.Areas.Club.Controllers
{

    public class HomeController : ClubBaseController
    {
        public IActionResult Index(int? clubId = 0, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return Redirect(returnUrl);
            }
            return View();
        }
    }
}
