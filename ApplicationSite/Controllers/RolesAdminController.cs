using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;
using ApplicationSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationSite.Controllers
{
    /// <summary>
    ///     Used to manage roles in the application. Only to be used by admin.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;


        public RolesAdminController()
        {
        }

        public RolesAdminController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }

        /// <summary>
        ///     See all roles in the database.
        /// </summary>
        /// <returns>An action result.</returns>
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        /// <summary>
        ///     See the details for a specific role.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <returns>An action result.</returns>
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = await RoleManager.FindByIdAsync(id);

            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (ApplicationUser user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        /// <summary>
        ///     Create new user role.
        /// </summary>
        /// <returns>An action result.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        ///     Create a new user role.
        ///     If we succeed, add the user role to the site, and allow admin to provision new users
        ///     with it. Else toss them back to the create screen with an error.
        /// </summary>
        /// <param name="roleViewModel">The role view model.</param>
        /// <returns>An Action Result.</returns>
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(roleViewModel.Name);
                IdentityResult roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return View();
                }
                // TODO: Show success screen? Is redirect to index enough to show it?
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        ///     Edit a users role.
        /// </summary>
        /// <param name="id">The id of the role you wish to edit.</param>
        /// <returns>An Action Result.</returns>
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            var roleModel = new RoleViewModel {Id = role.Id, Name = role.Name};
            return View(roleModel);
        }

        /// <summary>
        ///     Edit a users role.
        ///     If we succeed, edit the role. Else toss you back to
        ///     the role edit screen.
        /// </summary>
        /// <param name="roleModel">The role model.</param>
        /// <returns>An Action Result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = await RoleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                await RoleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        ///     Delete a user role.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <returns>An Action Result.</returns>
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        /// <summary>
        ///     Confirm deletion of user/role.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <param name="deleteUser">If we should delete the user.</param>
        /// <returns>An action result.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                IdentityRole role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result;
                if (deleteUser != null)
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                else
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}