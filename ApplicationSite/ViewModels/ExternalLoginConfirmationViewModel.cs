using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationSite.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Resources), Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Resources), Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Resources), Name = "LastName")]
        public string LastName { get; set; }
    }
}