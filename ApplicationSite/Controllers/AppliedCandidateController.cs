﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;

namespace ApplicationSite.Controllers
{
    public class AppliedCandidateController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AppliedCandidate
        public async Task<ActionResult> Index()
        {
            return View(await db.AppliedCandidates.ToListAsync());
        }

        // GET: AppliedCandidate/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppliedCandidates appliedCandidates = await db.AppliedCandidates.FindAsync(id);
            if (appliedCandidates == null)
            {
                return HttpNotFound();
            }
            return View(appliedCandidates);
        }

        // GET: AppliedCandidate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppliedCandidate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AppliedCandidateState")] AppliedCandidates appliedCandidates)
        {
            if (ModelState.IsValid)
            {
                db.AppliedCandidates.Add(appliedCandidates);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(appliedCandidates);
        }

        // GET: AppliedCandidate/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppliedCandidates appliedCandidates = await db.AppliedCandidates.FindAsync(id);
            if (appliedCandidates == null)
            {
                return HttpNotFound();
            }
            return View(appliedCandidates);
        }

        // POST: AppliedCandidate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AppliedCandidateState")] AppliedCandidates appliedCandidates)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appliedCandidates).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(appliedCandidates);
        }

        // GET: AppliedCandidate/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppliedCandidates appliedCandidates = await db.AppliedCandidates.FindAsync(id);
            if (appliedCandidates == null)
            {
                return HttpNotFound();
            }
            return View(appliedCandidates);
        }

        // POST: AppliedCandidate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AppliedCandidates appliedCandidates = await db.AppliedCandidates.FindAsync(id);
            db.AppliedCandidates.Remove(appliedCandidates);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}