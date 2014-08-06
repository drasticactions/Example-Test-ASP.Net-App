using System.Web.Mvc;

namespace ApplicationSite.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// The root of the website. The first page a user will hit.
        /// </summary>
        /// <returns>An action result.</returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
