using System.ComponentModel.DataAnnotations;

namespace Clubmates.Web.Models
{
    public class Poll
    {
        [Key]
        public int PollId { get; set; }
        public Club? Club { get; set; }
        public ClubEvent? ClubEvent { get; set; }
        public string? PollQuestion { get; set; }
        public string? PollDescription { get; set; }
        public DateTime? PollStartDateTime { get; set; }
        public DateTime? PollEndDateTime { get; set; }
        public bool? IsMultipleChoice { get; set; }
        public List<PollOption>? PollOptions { get; set; } = [];
    }
}
