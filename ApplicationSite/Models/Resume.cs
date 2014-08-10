using System.ComponentModel.DataAnnotations;

namespace ApplicationSite.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "Title")]
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "Filename")]
        [Required(AllowEmptyStrings = false)]
        public string FileName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Path { get; set; }
    }
}