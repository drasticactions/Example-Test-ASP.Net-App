using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;
using ApplicationSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationSite.Controllers
{
    public class PositionsListController : Controller
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
        // GET: PositionsList
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            var positions = _db.Positions.Where(node => node.PositionStatus == PositionStatus.Open).ToList();
            var positionListVm = new PositionsListViewModel();
            positionListVm.MapTo(positions);
            return View(positionListVm);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            var position = _db.Positions.Find(id);
            if (position.PositionStatus != PositionStatus.Open)
            {
                // If the user saved a link to the position detail page, and it has since been closed
                // We need to kick them out.
                // TODO: Create new error page telling the user that the position is closed.
                return RedirectToAction("Index");
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            var appliedCandidate = await _db.AppliedCandidates.FirstOrDefaultAsync(node => node.User.Id == user.Id && node.Position.Id == position.Id);
            var positionsDetailViewModel = new PositionsListDetailViewModel();
            bool hasApplied = appliedCandidate != null;
            positionsDetailViewModel.MapTo(position, hasApplied);
            return View(positionsDetailViewModel);
        }
    }
}