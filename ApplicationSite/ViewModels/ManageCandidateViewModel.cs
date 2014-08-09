using System.Collections.Generic;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class ManageCandidateViewModel
    {
        public List<AppliedCandidates> AppliedForPositions { get; set; }

        public List<Resume> Resumes { get; set; }

        public List<Resume> UnreadResumes { get; set; } 
    }
}