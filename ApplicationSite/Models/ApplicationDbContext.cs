using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ApplicationSite.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Positions> Positions { get; set; }

        public DbSet<Resume> Resumes { get; set; }

        public DbSet<AppliedCandidates> AppliedCandidates { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}