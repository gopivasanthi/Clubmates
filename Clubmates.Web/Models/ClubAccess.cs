using System.ComponentModel.DataAnnotations;

namespace Clubmates.Web.Models
{
    public class ClubAccess
    {
        [Key]
        public int ClubAccessId { get; set; }
        public Club? Club { get; set; }
        public ClubmatesUser? ClubmatesUser { get; set; }
        public ClubAccessRole? ClubAccessRole { get; set; }
    }

    public enum ClubAccessRole
    {
        ClubMember,
        ClubManager,
        ClubAdmin
    }

}
