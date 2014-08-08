using System.Web;
using ApplicationSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationSite.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationSite.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ApplicationSite.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationSite.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Employee"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Employee" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Candidate"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Candidate" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var test = new ApplicationUser { UserName = "Admin", Email = "Admin@ThisSite.com"};

                manager.Create(test, "123456");
                manager.AddToRole(test.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "TestEmployee"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var test = new ApplicationUser { UserName = "TestEmployee", Email = "TestEmployee@ThisSite.com" };

                manager.Create(test, "123456");
                manager.AddToRole(test.Id, "Employee");
            }

            if (!context.Users.Any(u => u.UserName == "TestUser"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var test = new ApplicationUser { UserName = "TestUser", Email = "TestUser@ThatSite.com" };

                manager.Create(test, "123456");
                manager.AddToRole(test.Id, "Candidate");
            }

            if (context.Positions.Any())
            {
                return;
            }

            for (var index = 0; index <= 3; index++)
            {
                var position = new Positions
                {
                    Description = "###Test Description " + index,
                    PositionStatus = PositionStatus.Open,
                    Title = "Test Title " + index
                };
                context.Positions.Add(position);
            }

            for (var index = 0; index <= 3; index++)
            {
                var position = new Positions
                {
                    Description = "###Test Closed Description " + index,
                    PositionStatus = PositionStatus.Closed,
                    Title = "Test Closed Title " + index
                };
                context.Positions.Add(position);
            }
        }
    }
}
