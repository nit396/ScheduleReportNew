using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ScheduleReport.App_Start;
using ScheduleReport.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScheduleReport.Controllers
{
   public class AccountController : Controller
   {
      private AppSignInManager _signInManager;
      private AppUserManager _userManager;

      public AccountController()
      {
      }

      public AccountController(AppUserManager userManager, AppSignInManager signInManager)
      {
         UserManager = userManager;
         SignInManager = signInManager;
      }

      [AllowAnonymous]
      public ActionResult Login()
      {
         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Login(LoginViewModel model)
      {
         if (!ModelState.IsValid)
         {
            return View(model);
         }

         // This doesn't count login failures towards account lockout
         // To enable password failures to trigger account lockout, change to shouldLockout: true
         var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
         if (result == SignInStatus.Failure)
         {
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
         }
         return RedirectToAction("Index", "Home");
      }

      public ActionResult LogOff()
      {
         AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
         return RedirectToAction("Login", "Account");
      }

      private IAuthenticationManager AuthenticationManager
      {
         get
         {
            return HttpContext.GetOwinContext().Authentication;
         }
      }

      public AppSignInManager SignInManager
      {
         get
         {
            return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
         }
         private set
         {
            _signInManager = value;
         }
      }

      public AppUserManager UserManager
      {
         get
         {
            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
         }
         private set
         {
            _userManager = value;
         }
      }
   }
}
