using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ApplicationSite.ViewModels
{
    public class ResumeViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ResumeTitle")]
        public string Title { get; set; }

        [DataType(DataType.Upload)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ResumeFilePdf")]
        public HttpPostedFileBase ResumeFile { get; set; }
    }
}