using System.Collections.Generic;
using System.ComponentModel;
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
    ///     Used to manage a users information.
    /// </summary>
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        /// <summary>
        ///     Sets up the user manager.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        /// <summary>
        ///     Gets the index page.
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
                return RedirectToAction("ManageEmployee", new {id = 0});
            }
            return RedirectToAction("ManageCandidate");
        }

         [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public ActionResult ManageEmployee([DefaultValue(0)] int id)
        {
            List<AppliedCandidates> appliedForPositions = _db.AppliedCandidates.ToList();
            List<Positions> positions = _db.Positions.ToList();

            var appliedCandidateStateOptions = (AppliedCandidateStateOptions) id;
            switch (appliedCandidateStateOptions)
            {
                case AppliedCandidateStateOptions.New:
                    appliedForPositions =
                        appliedForPositions.Where(node => node.AppliedCandidateState == AppliedCandidateStateOptions.New)
                            .ToList();
                    appliedCandidateStateOptions = AppliedCandidateStateOptions.New;
                    break;
                case AppliedCandidateStateOptions.Contact:
                    appliedForPositions =
                        appliedForPositions.Where(
                            node => node.AppliedCandidateState == AppliedCandidateStateOptions.Contact).ToList();
                    appliedCandidateStateOptions = AppliedCandidateStateOptions.Contact;
                    break;
                case AppliedCandidateStateOptions.Hire:
                    appliedForPositions =
                        appliedForPositions.Where(
                            node => node.AppliedCandidateState == AppliedCandidateStateOptions.Hire).ToList();
                    appliedCandidateStateOptions = AppliedCandidateStateOptions.Hire;
                    break;
                case AppliedCandidateStateOptions.Interview:
                    appliedForPositions =
                        appliedForPositions.Where(
                            node => node.AppliedCandidateState == AppliedCandidateStateOptions.Interview).ToList();
                    appliedCandidateStateOptions = AppliedCandidateStateOptions.Interview;
                    break;
                case AppliedCandidateStateOptions.Reject:
                    appliedForPositions =
                        appliedForPositions.Where(
                            node => node.AppliedCandidateState == AppliedCandidateStateOptions.Reject).ToList();
                    appliedCandidateStateOptions = AppliedCandidateStateOptions.Reject;
                    break;
                case AppliedCandidateStateOptions.Removed:
                    appliedForPositions =
                        appliedForPositions.Where(
                            node => node.AppliedCandidateState == AppliedCandidateStateOptions.Removed).ToList();
                    appliedCandidateStateOptions = AppliedCandidateStateOptions.Removed;
                    break;
            }
            var manageEmployeeVm = new ManageEmployeeViewModel();
            manageEmployeeVm.MapTo(positions, appliedForPositions, appliedCandidateStateOptions);
            return View(manageEmployeeVm);
        }

        [Authorize(Roles = "Admin,Employee,Candidate")]
        [HttpGet]
        public ActionResult ManageCandidate(string sortOrder)
        {
            string userId = User.Identity.GetUserId();
            var appliedForPositions =
                _db.AppliedCandidates.Where(node => node.User.Id.Equals(userId));
            List<Resume> resumes = _db.Resumes.Where(node => node.User.Id == userId && appliedForPositions.All(apply => apply.Resume.Id != node.Id)).ToList();
            List<Resume> unreadResumes = _db.Resumes.Where(node => node.User.Id == userId && !appliedForPositions.All(apply => apply.Resume.Id != node.Id)).ToList();
            var manageCandidateVm = new ManageCandidateViewModel
            {
                AppliedForPositions = appliedForPositions.OrderBy(node => node.AppliedTime).ToList(),
                Resumes = resumes,
                UnreadResumes = unreadResumes
            };
            return View(manageCandidateVm);
        }
    }
}