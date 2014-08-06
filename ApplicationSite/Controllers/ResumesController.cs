using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class ResumesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Resumes
        public async Task<ActionResult> Index()
        {
            return View(await _db.Resumes.ToListAsync());
        }

        // GET: Resumes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume resume = await _db.Resumes.FindAsync(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            return View(resume);
        }

        // GET: Resumes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resumes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title")] ResumeViewModel resume, HttpPostedFileBase resumeFile)
        {
            if (!ModelState.IsValid)
            {
                return View(resume);
            }

            if (resumeFile == null)
            {
                ModelState.AddModelError("", Resources.Resources.ResumeNotUploaded);
                return View(resume);
            }

            var currentUser = _db.Users.Find(User.Identity.GetUserId());

            // TODO: Placeholder for more advance check. Not sure if it's needed, but we don't
            // want users uploading things that are not pdfs.
            if (!resumeFile.ContentType.Equals("application/pdf"))
            {
                ModelState.AddModelError("", Resources.Resources.WrongFiletype);
                return View(resume);
            }

            var newResume = new Resume()
            {
                Title = resume.Title,
                FileName = resumeFile.FileName,
                User = (ApplicationUser)currentUser
            };

            if (!ModelState.IsValid) return View(resume);

            var resumeFilePath = string.Empty;
            if (resumeFile.ContentLength > 0)
            {
                // TODO: Only for testing, we are not uploading documents yet.
                var path = "/fake/path";
                resumeFilePath = path;
                //resumeFile.SaveAs(path);
            }
            newResume.Path = resumeFilePath;
            _db.Resumes.Add(newResume);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private ApplicationUserManager _userManager;

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

        // GET: Resumes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume resume = await _db.Resumes.FindAsync(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            return View(resume);
        }

        // POST: Resumes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,FileName,Path")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(resume).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(resume);
        }

        // GET: Resumes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume resume = await _db.Resumes.FindAsync(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
            return View(resume);
        }

        // POST: Resumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Resume resume = await _db.Resumes.FindAsync(id);
            _db.Resumes.Remove(resume);
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
