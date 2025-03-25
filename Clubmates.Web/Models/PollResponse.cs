using System.ComponentModel.DataAnnotations;

namespace Clubmates.Web.Models
{
    public class PollResponse
    {
        [Key]
        public int ResponseId { get; set; }
        public Poll? Poll { get; set; }
        public ClubmatesUser? ClubmatesUser { get; set; }
        public List<PollOption>? PollOptions { get; set; }
    }
}
