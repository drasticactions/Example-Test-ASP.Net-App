using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;
using ApplicationSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationSite.Controllers
{
    public class PositionsController : Controller
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


        // GET: Positions
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await _db.Positions.ToListAsync());
        }

        // GET: Positions/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
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
            var position = new PositionsViewModel()
            {
                Description = positions.Description,
                Id = positions.Id,
                Title = positions.Title
            };
            return View(position);
        }

        // GET: Positions/Create
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,PositionStatus")] PositionsViewModel positionVm)
        {
            if (!ModelState.IsValid)
            {
                return View(positionVm);
            }
            var position = new Positions();
            position.MapTo(positionVm.Id, positionVm.Title, positionVm.Description, positionVm.PositionStatus);
            _db.Positions.Add(position);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
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
            var position = new PositionsViewModel()
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,PositionStatus")] PositionsViewModel positionVm)
        {
            if (!ModelState.IsValid)
            {
                return View(positionVm);
            }
            var position = new Positions();
            position.MapTo(positionVm.Id, positionVm.Title, positionVm.Description, positionVm.PositionStatus);
            _db.Entry(position).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
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
            positionVm.MapTo(position.Id, position.Title, position.Description, (PositionStatus)position.PositionStatus);
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
            // TODO: Move model mapping to an actual model mapper.
            // This is to test and make sure everything works.

            var position = await _db.Positions.FindAsync(id);
            var resumes = await _db.Resumes.ToListAsync();
            var list = resumes.Select(resume => new SelectListItem {Value = resume.Id.ToString(CultureInfo.InvariantCulture), Text = resume.Title}).ToList();
            var selectList = new SelectList(list, "Value", "Text", 0);
            var appliedCandidateViewModel = new AppliedCandidateViewModel()
            {
                DefaultSelectItem = 0,
                Position = position,
                ResumeSelectList = selectList
            };
            return View(appliedCandidateViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Employee, Candidate")]
        public async Task<ActionResult> Apply(AppliedCandidateViewModel appliedCandidateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(appliedCandidateViewModel);
            }
            // TODO: These are hard coded and are not using the view models
            // Instead we are just pulling the first value from the database.
            // This is only for testing, it needs to be updated.

            var resume = _db.Resumes.First();
            var position = _db.Positions.First();
            var appliedCanidate = new AppliedCandidates()
            {
                AppliedCandidateState = (int)AppliedCandidateStateOptions.New,
                Position = position,
                Resume = resume,
                User = null
            };
            _db.AppliedCandidates.Add(appliedCanidate);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
