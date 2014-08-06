using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class ResumeViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        // TODO: Remove from view model, it should be posted in the create form.
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ResumeFile { get; set; }
    }
}