using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationSite.Models
{
    public class AppliedCandidates
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Resume Resume { get; set; }

        public virtual Positions Position { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "CandidateStatus")]
        public AppliedCandidateStateOptions AppliedCandidateState { get; set; }

        public DateTime AppliedTime { get; set; }
    }

    /// <summary>
    ///     The state of the application of the candidate.
    /// </summary>
    public enum AppliedCandidateStateOptions
    {
        New,
        Contact,
        Interview,
        Hire,
        Removed,
        Reject
    }
}