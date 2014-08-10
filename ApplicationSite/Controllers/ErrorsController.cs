using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationSite.Controllers
{
    public class ErrorsController : Controller
    {
        /// <summary>
        /// Shows the internal error page.
        /// </summary>
        /// <returns>An action result.</returns>
        public ActionResult InternalError()
        {
            // NOTE: This is a basic view, with no error information. If an error occurrs, we should capture it.
            // so we can figure out what went wrong.
            return View();
        }

        /// <summary>
        /// Shows the 404 page not found.
        /// </summary>
        /// <returns>An action result.</returns>
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}