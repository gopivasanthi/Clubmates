using Clubmates.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clubmates.Web.AppDbContext
{
    public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) 
                        : IdentityDbContext<ClubmatesUser>(options)
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubAccess> ClubAccesses { get; set; }
        public DbSet<ClubEvent> ClubEvents { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<PollResponse> PollResponses { get; set; }
    }
}
