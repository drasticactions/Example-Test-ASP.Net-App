using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class AppliedCandidateViewModel
    {
        [Display(Name = "Resumes")]
        public SelectList ResumeSelectList { get; set; }

        public int DefaultSelectItem = 0;

        [Display(Name = "Position")]
        public Positions Position { get; set; }
    }
}