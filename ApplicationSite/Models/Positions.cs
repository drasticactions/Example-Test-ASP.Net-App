using System.ComponentModel.DataAnnotations;

namespace ApplicationSite.Models
{
    public class Positions
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        public int PositionStatus { get; set; }
    }

    public enum PositionStatus
    {
        Open,
        Closed
    }
}