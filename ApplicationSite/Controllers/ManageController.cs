using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationSite.Controllers
{
    /// <summary>
    /// Used to manage a users information.
    /// </summary>
    [Authorize]
    public class ManageController : Controller
    {
        /// <summary>
        /// Gets the index page.
        /// </summary>
        /// <returns>An action result.</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}