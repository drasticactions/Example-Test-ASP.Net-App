using System.ComponentModel.DataAnnotations;

namespace ApplicationSite.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Email")]
        public string Email { get; set; }


        [Required]
        [Display(ResourceType = typeof (Resources.Resources), Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(ResourceType = typeof (Resources.Resources), Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resources.Resources),
            ErrorMessageResourceName = "PasswordLengthError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof (Resources.Resources),
            ErrorMessageResourceName = "PasswordConfirmationError")]
        public string ConfirmPassword { get; set; }
    }
}