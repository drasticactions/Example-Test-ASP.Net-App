using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ApplicationSite.Models;

namespace ApplicationSite.Controllers
{
    /// <summary>
    /// The action controller used to 
    /// </summary>
    [Authorize(Roles = "Admin, Employee")]
    public class AppliedCandidateController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        /// <summary>
        /// Edits the applied candidates status.
        /// </summary>
        /// <param name="id">The applied candidate id.</param>
        /// <returns>An action result.</returns>
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppliedCandidates appliedCandidates = await _db.AppliedCandidates.FindAsync(id);
            if (appliedCandidates == null)
            {
                return HttpNotFound();
            }
            return View(appliedCandidates);
        }

        // POST: AppliedCandidate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edits the applied candidate.
        /// </summary>
        /// <param name="appliedCandidates">The applied candidate model.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,AppliedCandidateState,AppliedTime")] AppliedCandidates appliedCandidates)
        {
            if (!ModelState.IsValid) return View(appliedCandidates);
            _db.Entry(appliedCandidates).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Manage");
        }

        /// <summary>
        /// Deletes the applied candidate (IE. An employee wants to revoke their application)
        /// </summary>
        /// <param name="id">The applied candidate id.</param>
        /// <returns>An action result.</returns>
        [Authorize(Roles = "Admin,Employee,Candidate")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppliedCandidates appliedCandidates = await _db.AppliedCandidates.FindAsync(id);
            if (appliedCandidates == null)
            {
                return HttpNotFound();
            }
            return View(appliedCandidates);
        }

        /// <summary>
        /// Delete the applied candidate row.
        /// </summary>
        /// <param name="id">The applied candidate id.</param>
        /// <returns>An action result.</returns>
        [Authorize(Roles = "Admin,Employee,Candidate")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AppliedCandidates appliedCandidates = await _db.AppliedCandidates.FindAsync(id);
            _db.AppliedCandidates.Remove(appliedCandidates);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Manage");
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