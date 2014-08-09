using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationSite.Models;
using ApplicationSite.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationSite.Controllers
{
    /// <summary>
    ///     Used to manage users in the application. Only to be used by admin.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;

        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }

        /// <summary>
        ///     Shows the list of users in the database.
        /// </summary>
        /// <returns>An action result.</returns>
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        /// <summary>
        ///     Shows details of a specific user.
        /// </summary>
        /// <param name="id">The users id.</param>
        /// <returns>An action result.</returns>
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        /// <summary>
        ///     Create a new user.
        /// </summary>
        /// <returns>An action result.</returns>
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        /// <summary>
        ///     Create a new user.
        ///     If we succeed, create the user and return to login.
        ///     Else return to the create user view with errors.
        /// </summary>
        /// <param name="userViewModel">The user view model.</param>
        /// <param name="selectedRoles">The selected roles of the user.</param>
        /// <returns>An action result.</returns>
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Username,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName
                };
                IdentityResult adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles == null)
                    {
                        return RedirectToAction("Index");
                    }

                    IdentityResult result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", result.Errors.First());
                    ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                    return View();
                }
                ModelState.AddModelError("", adminresult.Errors.First());
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                return View();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        /// <summary>
        ///     Edit a user.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns>An action result</returns>
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            IList<string> userRoles = await UserManager.GetRolesAsync(user.Id);

            return View(new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        /// <summary>
        ///     Edit a user.
        ///     If we succeed, edit the user and return to index,
        ///     else go back to the edit screen with errors.
        /// </summary>
        /// <param name="editUserViewModel">The edit user view model</param>
        /// <param name="selectedRole">The selected roles of the user.</param>
        /// <returns>An action result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] EditUserViewModel editUserViewModel,
            params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(editUserViewModel.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUserViewModel.Email;
                user.Email = editUserViewModel.Email;

                IList<string> userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] {};

                IdentityResult result =
                    await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray());

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Errors.First());
                return View();
            }
            ModelState.AddModelError("", Resources.Resources.UsersAdminController_Edit_Something_failed_);
            return View();
        }

        /// <summary>
        ///     Delete a user.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns>An action result.</returns>
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        ///     Confirm deleting a user.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns>An action result.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            // If the model state is not valid, return right away.
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            IdentityResult result = await UserManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Errors.First());
            return View();
        }
    }
}