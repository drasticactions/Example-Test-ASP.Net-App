using System.Web.Mvc;
using ApplicationSite.Models;

namespace ApplicationSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        /// <summary>
        ///     The root of the website. The first page a user will hit.
        /// </summary>
        /// <returns>An action result.</returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}