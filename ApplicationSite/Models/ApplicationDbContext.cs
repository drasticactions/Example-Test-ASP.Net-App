using System.Data.Entity;
using ApplicationSite.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ApplicationSite.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Positions> Positions { get; set; }

        public System.Data.Entity.DbSet<ApplicationSite.Models.Resume> Resumes { get; set; }

        public System.Data.Entity.DbSet<ApplicationSite.Models.AppliedCandidates> AppliedCandidates { get; set; }
    }
}