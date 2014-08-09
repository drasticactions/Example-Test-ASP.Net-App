using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ApplicationSite.Models
{
    /// <summary>
    ///     The base user of our application.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        ///     The users first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     The users last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Generate the users ASP.net Identity.
        ///     Taken from MVC 5 Identity sample.
        /// </summary>
        /// <param name="manager">The user manager.</param>
        /// <returns>A user identity.</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity =
                await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}