using System.Globalization;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.error = false;
            ViewBag.pwd = false;

            if (!ModelState.IsValid)
            {
                ViewBag.error = true;
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
           
            if (result == SignInStatus.Success)
            {
                return RedirectToAction("RedirectToController", "Account");
            }
            else
            {
                ModelState.AddModelError("", "No existe el usuario o la contraseña.");
                return View(model);
            }
        }

        //
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("RedirectToController");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo cambiar la contraseña");
                    return View();
                }
            }

            return View();
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            return LogOff(1);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff(int var = 0)
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public async Task<ActionResult> RedirectToController()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                // Implementar un manejador de errores
                return HttpNotFound("Usuario no encontrado");
            }

            var rol = (await UserManager.GetRolesAsync(user.Id))
                        .ToList()
                        .Single();


            if (rol.Equals("Admin"))
                return RedirectToAction("Index", "Admin");
            else if (rol.Equals("Profesor"))
                return RedirectToAction("Index", "Profesor");
            else if (rol.Equals("Coordinador"))
                return RedirectToAction("Index", "Coordinador");
            else
                return HttpNotFound("Error al elegir el rol del usuario");
        }

        #endregion
    }
}