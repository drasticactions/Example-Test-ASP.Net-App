using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using ApplicationSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationSite
{
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEf(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEf(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string email = "admin@admin.com";
            const string name = "Admin";
            const string password = "123456";
            const string roleName = "Admin";

            IdentityRole role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }

            ApplicationUser user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser {UserName = name, Email = email};
                userManager.Create(user, password);
                userManager.SetLockoutEnabled(user.Id, false);
            }

            IList<string> rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
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

            for (int index = 0; index <= 3; index++)
            {
                var testUser = new ApplicationUser
                {
                    Email = "testemail@email.com",
                    UserName = "Test Employee " + index
                };
                userManager.Create(testUser, password);
                userManager.SetLockoutEnabled(testUser.Id, false);
                IdentityRole employeeRole = roleManager.FindByName("Employee");
                userManager.AddToRole(testUser.Id, employeeRole.Id);
            }

            for (int index = 0; index <= 3; index ++)
            {
                var position = new Positions
                {
                    Description = "Test Description " + index,
                    PositionStatus = PositionStatus.Open,
                    Title = "Test Title " + index
                };
                db.Positions.Add(position);
            }

            for (int index = 0; index <= 3; index++)
            {
                var position = new Positions
                {
                    Description = "Test Closed Description " + index,
                    PositionStatus = PositionStatus.Closed,
                    Title = "Test Closed Title " + index
                };
                db.Positions.Add(position);
            }
        }
    }
}