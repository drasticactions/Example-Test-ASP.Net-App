using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationSite.Models;

namespace ApplicationSite.ViewModels
{
    public class ManageEmployeeViewModel
    {
        public List<Positions> AllPositions { get; set; }

        public List<AppliedCandidates> AllAppliedCandidates { get; set; }

        public AppliedCandidateStateOptions AppliedCandidateStateOptions { get; set; }

        public void MapTo(List<Positions> positions, List<AppliedCandidates> candidates, AppliedCandidateStateOptions option)
        {
            AllPositions = positions;
            AllAppliedCandidates = candidates;
            AppliedCandidateStateOptions = option;
        }
    }
}