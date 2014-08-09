using System.ComponentModel.DataAnnotations;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class PositionsViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "PositionStatus")]
        public PositionStatus PositionStatus { get; set; }

        public void MapTo(int id, string title, string description, PositionStatus positionStatus)
        {
            Id = id;
            Title = title;
            Description = description;
            PositionStatus = positionStatus;
        }
    }
}