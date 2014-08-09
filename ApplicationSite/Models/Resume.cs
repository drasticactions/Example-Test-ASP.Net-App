using System.ComponentModel.DataAnnotations;

namespace ApplicationSite.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FileName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Path { get; set; }
    }
}