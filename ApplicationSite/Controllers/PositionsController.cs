using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ApplicationSite.Models;
using ApplicationSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Postal;

namespace ApplicationSite.Controllers
{
    public class PositionsController : Controller
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

        [HttpGet]
        [Authorize(Roles = "Admin, Employee, Candidate")]
        public ActionResult ApplySuccess()
        {
            return View();
        }

        // GET: Positions
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Index()
        {
            return RedirectToAction("ManageEmployee", "Manage");
        }

        // GET: Positions/Create
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Title,Description,PositionStatus")] PositionsViewModel positionVm)
        {
            if (!ModelState.IsValid)
            {
                return View(positionVm);
            }
            var position = new Positions();
            position.MapTo(positionVm.Id, positionVm.Title, positionVm.Description, positionVm.PositionStatus);
            _db.Positions.Add(position);
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageEmployee", "Manage");
        }

        // GET: Positions/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Positions positions = await _db.Positions.FindAsync(id);
            if (positions == null)
            {
                return HttpNotFound();
            }
            var position = new PositionsViewModel
            {
                Description = positions.Description,
                Id = id,
                Title = positions.Title
            };
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Title,Description,PositionStatus")] PositionsViewModel positionVm)
        {
            if (!ModelState.IsValid)
            {
                return View(positionVm);
            }

            var appliedCandidates = _db.AppliedCandidates.Where(node => node.Position.Id == positionVm.Id).ToList();
            var oldPosition = _db.Positions.Find(positionVm.Id);
            if (positionVm.PositionStatus == PositionStatus.Closed && oldPosition.PositionStatus == PositionStatus.Open)
            {
                // Filter all appliedCandidates who are not "hired", and tell them that the position is closed.
                appliedCandidates =
                    appliedCandidates.Where(node => node.AppliedCandidateState != AppliedCandidateStateOptions.Hire).ToList();
                this.SendEmail(appliedCandidates, oldPosition, positionVm, "PositionClosed");
            }
            else if (positionVm.PositionStatus == PositionStatus.Open &&
                     oldPosition.PositionStatus == PositionStatus.Closed)
            {
                // Tell all candidates who have not been contacted yet (or were rejected) that the position has been reopened.
                appliedCandidates =
                    appliedCandidates.Where(node => node.AppliedCandidateState != AppliedCandidateStateOptions.Hire 
                        && node.AppliedCandidateState != AppliedCandidateStateOptions.Reject 
                        && node.AppliedCandidateState != AppliedCandidateStateOptions.Removed).ToList();
                this.SendEmail(appliedCandidates, oldPosition, positionVm, "PositionReopened");
            }
            else if (positionVm.PositionStatus == PositionStatus.Open)
            {
                // Send "Position Changed" email to all.
                this.SendEmail(appliedCandidates, oldPosition, positionVm, "PositionChanged");
            }
            oldPosition.MapTo(positionVm.Id, positionVm.Title, positionVm.Description, positionVm.PositionStatus);
            _db.Entry(oldPosition).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageEmployee", "Manage");
        }



        public void SendEmail(List<AppliedCandidates> appliedCandidates, Positions oldPosition,
            PositionsViewModel positionVm, string view)
        {
            if (!appliedCandidates.Any()) return;
            foreach (var candidate in appliedCandidates)
            {
                _db.Entry<AppliedCandidates>(candidate).Reference("User").Load();
                dynamic email = new Email(view);
                email.To = candidate.User.Email;
                email.FirstName = candidate.User.FirstName;
                email.PositionTitle = oldPosition.Title;
                email.PositionId = positionVm.Id;
                email.Send();
            }
        }

        // GET: Positions/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Positions position = await _db.Positions.FindAsync(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            var positionVm = new PositionsViewModel();
            positionVm.MapTo(position.Id, position.Title, position.Description, position.PositionStatus);
            return View(positionVm);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Positions positions = await _db.Positions.FindAsync(id);
            _db.Positions.Remove(positions);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Employee, Candidate")]
        [HttpGet]
        public async Task<ActionResult> Apply(int id)
        {
            // If user enters apply URL directly, do server side validation to see if they have already applied.
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            AppliedCandidates appliedCanidate =
                await
                    _db.AppliedCandidates.FirstOrDefaultAsync(node => node.User.Id == user.Id && node.Position.Id == id);
            if (appliedCanidate != null)
            {
                // TODO: Instead of just sending to index, send to error page saying they have already applied.
                return RedirectToAction("Index", "PositionsList");
            }

            Positions position = await _db.Positions.FindAsync(id);
            List<Resume> resumes = await _db.Resumes.ToListAsync();
            List<SelectListItem> list =
                resumes.Where(resume => resume.User.Id == user.Id)
                    .Select(
                        resume =>
                            new SelectListItem
                            {
                                Value = resume.Id.ToString(CultureInfo.InvariantCulture),
                                Text = resume.Title
                            })
                    .ToList();
            var selectList = new SelectList(list, "Value", "Text", 0);
            var appliedCandidateViewModel = new AppliedCandidateViewModel();
            appliedCandidateViewModel.MapTo(selectList, position, user);
            return PartialView(appliedCandidateViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Employee, Candidate")]
        public async Task<ActionResult> Apply(
            [Bind(Include = "DefaultSelectItem,Position,CurrentUser")] AppliedCandidateViewModel
                appliedCandidateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(appliedCandidateViewModel);
            }
            IdentityUser user = await _db.Users.FirstAsync(node => node.Id == appliedCandidateViewModel.CurrentUser.Id);
            Positions position =
                await _db.Positions.FirstAsync(node => node.Id == appliedCandidateViewModel.Position.Id);

            AppliedCandidates appliedCanidateCheck =
                await
                    _db.AppliedCandidates.FirstOrDefaultAsync(
                        node => node.User.Id == user.Id && node.Position.Id == position.Id);
            if (appliedCanidateCheck != null)
            {
                // Doing another check if the user has already applyed for a position.
                // The chance is slim that they would end up here
                // (For example, they could have had two sessions open with the same apply button active, and then clicked on both), 
                // but the call is cheap, so it's worth doing just in case.
                return RedirectToAction("Index", "PositionsList");
            }

            Resume resume = await _db.Resumes.FirstAsync(node => node.Id == appliedCandidateViewModel.DefaultSelectItem);
            var appliedCanidate = new AppliedCandidates
            {
                AppliedCandidateState = AppliedCandidateStateOptions.New,
                Position = position,
                Resume = resume,
                AppliedTime = DateTime.Now,
                User = (ApplicationUser) user
            };
            _db.AppliedCandidates.Add(appliedCanidate);
            await _db.SaveChangesAsync();
            return RedirectToAction("ApplySuccess", "Positions");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee, Candidate")]
        public async Task<ActionResult> Withdraw(int id)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            AppliedCandidates appliedCandidate =
                await
                    _db.AppliedCandidates.FirstOrDefaultAsync(node => node.User.Id == user.Id && node.Position.Id == id);
            if (appliedCandidate == null)
            {
                // The user does not have a position request on file,
                // So we need to kick them out of here, since there is nothing to remove.
                // TODO: Send the user to an error page explaining what happened.
                return RedirectToAction("Index", "PositionsList");
            }
            return PartialView(appliedCandidate);
        }

        [HttpPost, ActionName("Withdraw")]
        [Authorize(Roles = "Admin, Employee, Candidate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WithdrawConfirmed(int id)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            AppliedCandidates appliedCandidate =
                await
                    _db.AppliedCandidates.FirstOrDefaultAsync(node => node.User.Id == user.Id && node.Position.Id == id);
            if (appliedCandidate == null)
            {
                // The user does not have a position request on file,
                // So we need to kick them out of here, since there is nothing to remove.
                // TODO: Send the user to an error page explaining what happened.
                return RedirectToAction("Index", "PositionsList");
            }
            try
            {
                _db.AppliedCandidates.Remove(appliedCandidate);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                // TODO: Send user to error page, telling them their application was not withdrawn.
                return RedirectToAction("Index", "PositionsList");
            }
            return RedirectToAction("WithdrawSuccess", "Positions");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee, Candidate")]
        public ActionResult WithdrawSuccess()
        {
            return View();
        }
    }
}