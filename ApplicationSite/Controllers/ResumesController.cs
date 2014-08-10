using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;
using ApplicationSite.Tools;
using ApplicationSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ApplicationSite.Controllers
{
    [Authorize]
    public class ResumesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> Index()
        {
            return RedirectToAction("ManageEmployee", "Manage");
        }

        // GET: Resumes/Details/5
        [Authorize(Roles = "Admin,Employee")]
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
        public PartialViewResult Create()
        {
            return PartialView("Create", new ResumeViewModel());
        }

        // POST: Resumes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title")] ResumeViewModel resume,
            HttpPostedFileBase resumeFile)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ManageCandidate", "Manage");
            }

            if (resumeFile == null)
            {
                TempData["ErrorMessage"] = Resources.Resources.ResumeNotUploaded;
                return RedirectToAction("ManageCandidate", "Manage");
            }

            IdentityUser currentUser = _db.Users.Find(User.Identity.GetUserId());

            // TODO: Placeholder for more advance check. Not sure if it's needed, but we don't
            // want users uploading things that are not pdfs.
            if (!resumeFile.ContentType.Equals("application/pdf"))
            {
                TempData["ErrorMessage"] = Resources.Resources.WrongFiletype;
                return RedirectToAction("ManageCandidate", "Manage");
            }

            var newResume = new Resume
            {
                Title = resume.Title,
                FileName = Path.GetFileName(resumeFile.FileName),
                User = (ApplicationUser) currentUser
            };

            string resumeFilePath = string.Empty;
            if (resumeFile.ContentLength > 0)
            {
                string path = Path.GetRandomFileName();
                var cloudStorage = new CloudStorage("resume", false);
                cloudStorage.UploadFile(resumeFile, path);
                resumeFilePath = path;
            }
            newResume.Path = resumeFilePath;
            _db.Resumes.Add(newResume);
            await _db.SaveChangesAsync();
            // TODO: Instead of a redirect, return what actually changed and update the caller.
            return RedirectToAction("ManageCandidate", "Manage");
        }

        // GET: Resumes/Edit/5
        [Authorize(Roles = "Admin,Employee")]
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
            if (!ModelState.IsValid)
            {
                return View(resume);
            }

            _db.Entry(resume).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Resumes/Delete/5

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resume resume = await _db.Resumes.FindAsync(id);

            // If a user tries to enter a 

            if (User.IsInRole("Candidate"))
            {
                if (resume.User.Id != User.Identity.GetUserId())
                {
                    return HttpNotFound();
                }
            }
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
            try
            {
                var cloudStorage = new CloudStorage("resume", false);
                _db.Resumes.Remove(resume);
                await _db.SaveChangesAsync();
                cloudStorage.DeleteFile(resume.Path);
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException)
            {
                // Resume is attached to an open position, so we can't delete it.
                // TODO: Make better error messages.
                ModelState.AddModelError("", Resources.Resources.ResumeDeleteOpenPositionError);
                return View(resume);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", Resources.Resources.ErrorMessageDeleteGeneric);
                return View(resume);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Download(int id)
        {
            // TODO: Add error handling if we can't find the file.
            Resume resume = await _db.Resumes.FindAsync(id);
            if (resume == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Candidate"))
            {
                if (resume.User.Id != User.Identity.GetUserId())
                {
                    return HttpNotFound();
                }
            }

            try
            {
                var cloudStorage = new CloudStorage("resume", false);
                CloudBlockBlob blob = cloudStorage.GetBlob(resume.Path);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + resume.FileName);
                blob.DownloadToStream(Response.OutputStream);
            }
            catch (Exception)
            {
                // TODO: Send to error page.
                return HttpNotFound();
            }

            return new EmptyResult();
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