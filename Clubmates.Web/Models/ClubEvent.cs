using System.ComponentModel.DataAnnotations;

namespace Clubmates.Web.Models
{
    public class ClubEvent
    {
        [Key]
        public int Id { get; set; }
        public Club? Club { get; set; }
        public string? EventName { get; set; }
        public string? EventDescription { get; set; }
        public string? EventLocation { get; set; }
        public DateTime? EventStartDateTime { get; set; }
        public DateTime? EventEndDateTime { get; set; }

    }
}
