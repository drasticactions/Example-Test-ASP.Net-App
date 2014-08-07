using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationSite.Models
{
    public class AppliedCandidates
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Resume Resume { get; set; }

        public virtual Positions Position { get; set; }

        public AppliedCandidateStateOptions AppliedCandidateState { get; set; }
    }

    /// <summary>
    /// The state of the application of the candidate.
    /// </summary>
    public enum AppliedCandidateStateOptions
    {
        None,
        New,
        Contact,
        Interview,
        Hire,
        Removed,
        Reject
    }
}