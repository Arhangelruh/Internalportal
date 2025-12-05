using InternalPortal.Web.Interfaces;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISignInManager _signInManager;

        public AccountController(
            ISignInManager signInManager
            )
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Login model.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>Login view</returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Login result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.SignIn(model.UserName, model.Password, ModelState);
                if (result)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }               
            }
            return View(model);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Home view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

    }
}
