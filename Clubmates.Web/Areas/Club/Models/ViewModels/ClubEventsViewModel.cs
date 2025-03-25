namespace Clubmates.Web.Areas.Club.Models.ViewModels
{
    public class ClubEventsViewModel
    {
        public int EventId { get; set; }
        public int? ClubId { get; set; }
        public string? ClubName { get; set; }
        public string? EventName { get; set; }
        public string? EventDescription { get; set; }
        public string? EventLocation { get; set; }
        public DateTime? EventStartDateTime { get; set; }
        public DateTime? EventEndDateTime { get; set; }

    }
}
