using Clubmates.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Clubmates.Web.AppDbContext
{
    public class AppIdentityDbContext : IdentityDbContext<ClubmatesUser>
    {
    }
}
