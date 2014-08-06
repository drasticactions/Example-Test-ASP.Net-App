using System.ComponentModel.DataAnnotations;

namespace ApplicationSite.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "RoleName")]
        public string Name { get; set; }
    }
}