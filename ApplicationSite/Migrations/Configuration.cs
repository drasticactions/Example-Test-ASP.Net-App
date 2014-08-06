using ApplicationSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ApplicationSite.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ApplicationSite.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {

            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
            }

            if (context.Users.Any())
            {
                // If we have users, we don't need to add Admin.
                return;
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = new ApplicationUser
            {
                Email = "Admin@admin.com",
                UserName = "Admin",
                FirstName = "Administrator",
                LastName = "User"
            };
            userManager.Create(user, "mansion96");
            userManager.AddToRole(user.Id, "Admin");
        }
    }
}
