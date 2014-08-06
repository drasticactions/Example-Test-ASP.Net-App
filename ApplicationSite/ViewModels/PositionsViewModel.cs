using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class PositionsViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(ResourceType = typeof(Resources.Resources), Name = "Title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(ResourceType = typeof(Resources.Resources), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "PositionStatus")]
        public PositionStatus PositionStatus { get; set; }
    }
}