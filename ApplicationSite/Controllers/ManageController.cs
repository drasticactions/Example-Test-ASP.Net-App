using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationSite.Controllers
{
    /// <summary>
    /// Used to manage a users information.
    /// </summary>
    [Authorize]
    public class ManageController : Controller
    {
        /// <summary>
        /// Gets the index page.
        /// </summary>
        /// <returns>An action result.</returns>
        [Authorize(Roles = "Admin,Employee,Candidate")]
        public ActionResult Index()
        {
            // If the user is an employee or the admin, launch the employee admin page
            // or else it's a normal user, so launch the candidate management page.
            if (User.IsInRole("Employee") || User.IsInRole("Admin"))
            {
               return RedirectToAction("ManageEmployee");
            }
           return RedirectToAction("ManageCandidate");
        }

        public ActionResult ManageEmployee()
        {
            return View();
        }

        public ActionResult ManageCandidate()
        {
            return View();
        }
    }
}