using System.ComponentModel.DataAnnotations;

namespace ApplicationSite.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof (Resources.Resources), Name = "LoginViewModel_Username_Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "LoginViewModel_Password_Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "LoginViewModel_RememberMe_Remember_me_")]
        public bool RememberMe { get; set; }
    }
}