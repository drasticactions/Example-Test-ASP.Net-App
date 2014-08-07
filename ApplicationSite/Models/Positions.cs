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

        public PositionStatus PositionStatus { get; set; }

        public void MapTo(int id, string title, string description, PositionStatus positionStatus)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.PositionStatus = positionStatus;
        }
    }

    public enum PositionStatus
    {
        Open,
        Closed
    }
}