using System.ComponentModel.DataAnnotations;

namespace Clubmates.Web.Models
{
    public class PollOption
    {
        [Key]
        public int PollOptionId { get; set; }
        public string? PollOptionText { get; set; }
    }
}
