using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;
using ApplicationSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationSite.Controllers
{
    /// <summary>
    /// Used to manage a users information.
    /// </summary>
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Sets up the user manager.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        /// <summary>
        /// Gets the index page.
        /// </summary>
        /// <returns>An action result.</returns>
        [Authorize(Roles = "Admin,Employee,Candidate")]
        [HttpGet]
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

        [HttpGet]
        public ActionResult ManageEmployee()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageCandidate()
        {
            string userId = User.Identity.GetUserId();
            var appliedForPositions = _db.AppliedCandidates.Where(node => node.User.Id.Equals(userId)).ToList();
            var resumes = _db.Resumes.Where(node => node.User.Id.Equals(userId)).ToList();
            var manageCandidateVm = new ManageCandidateViewModel
            {
                AppliedForPositions = appliedForPositions,
                Resumes = resumes
            };
            return View(manageCandidateVm);
        }
    }
}