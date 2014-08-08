using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ApplicationSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationSite
{
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext> 
    {
        protected override void Seed(ApplicationDbContext context) {
            InitializeIdentityForEf(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEf(ApplicationDbContext db) {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string email = "admin@admin.com";
            const string name = "Admin";
            const string password = "123456";
            const string roleName = "Admin";

            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null) {
                user = new ApplicationUser { UserName = name, Email = email };
                userManager.Create(user, password);
                userManager.SetLockoutEnabled(user.Id, false);
            }

            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name)) {
                userManager.AddToRole(user.Id, role.Name);
            }

            // Other User Roles
            role = roleManager.FindByName("Employee");
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }

            role = roleManager.FindByName("Candidate");
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }
        }
    }
}