using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class AppliedCandidateViewModel
    {
        [Display(Name = "Resumes")]
        public SelectList ResumeSelectList { get; set; }

        public int DefaultSelectItem { get; set; }

        [Display(Name = "Position")]
        public Positions Position { get; set; }

        public ApplicationUser CurrentUser { get; set; }

        public void MapTo(SelectList resumeSelectList, Positions postion, ApplicationUser user)
        {
            DefaultSelectItem = 0;
            Position = postion;
            ResumeSelectList = resumeSelectList;
            CurrentUser = user;
        }
    }
}