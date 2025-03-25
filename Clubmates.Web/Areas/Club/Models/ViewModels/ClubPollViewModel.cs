namespace Clubmates.Web.Areas.Club.Models.ViewModels
{
    public class ClubPollViewModel
    {
        public int PollId { get; set; }
        public int? ClubId { get; set; }
        public int? EventId { get; set; }
        public string? ClubName { get; set; }
        public string? EventName { get; set; }
        public string? PollQuestion { get; set; }
        public string? PollDescription { get; set; }
        public DateTime? PollStartDateTime { get; set; }
        public DateTime? PollEndDateTime { get; set; }
        public bool? IsMultipleChoice { get; set; }

    }
}
