using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;
using ApplicationSite.ViewModels;

namespace ApplicationSite.Controllers
{
    public class PositionsListController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

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
        public ActionResult Details(int id)
        {
            var position = _db.Positions.Find(id);
            if (position.PositionStatus != PositionStatus.Open)
            {
                // If the user saved a link to the position detail page, and it has since been closed
                // We need to kick them out.
                // TODO: Create new error page telling the user that the position is closed.
                return RedirectToAction("Index");
            }
            return View(position);
        }
    }
}