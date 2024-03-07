using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternalPortal.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        }

        /// <summary>
        /// Get profile.
        /// </summary>
        /// <returns>Profile model</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profileSID = User.Claims.Where(claim => claim.Type == ClaimTypes.Sid).Select(claim => claim.Value).SingleOrDefault();
            var profile = await _profileService.GetProfileByUserSIDAsync(profileSID);

            if (profile != null)
            {
                var model = new ProfileViewModel
                {
                    Name = profile.Name,
                    LastName = profile.LastName,
                    MiddleName = profile.MiddleName
                };

                return View(model);
            }
            ViewBag.ErrorMessage = "Пользователь не найден.";
            ViewBag.ErrorTitle = "Ошибка";
            return View("~/Views/Error/Error.cshtml");
        }

        /// <summary>
        /// Change profile.
        /// </summary>
        /// <returns>Profile model</returns>
        public async Task<IActionResult> EditProfile()
        {
            var profileSID = User.Claims.Where(claim => claim.Type == ClaimTypes.Sid).Select(claim => claim.Value).SingleOrDefault();
            var profile = await _profileService.GetProfileByUserSIDAsync(profileSID);
            if (profile != null)
            {
                var model = new ProfileViewModel
                {
                    Name = profile.Name,
                    LastName = profile.LastName,
                    MiddleName = profile.MiddleName
                };
                return View(model);
            }
            ViewBag.ErrorMessage = "Пользователь не найден.";
            ViewBag.ErrorTitle = "Ошибка";
            return View("~/Views/Error/Error.cshtml");
        }

        /// <summary>
        /// Edit user profile.
        /// </summary>
        /// <param name="editProfile"></param>
        /// <returns>Profile</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel editProfile)
        {
            var profileSID = User.Claims.Where(claim => claim.Type == ClaimTypes.Sid).Select(claim => claim.Value).SingleOrDefault();
            var getProfile = await _profileService.GetProfileByUserSIDAsync(profileSID);

            if (getProfile != null)
            {
                var profile = new Profile
                {
                    Id = getProfile.Id,
                    Name = editProfile.Name,
                    LastName = editProfile.LastName,
                    MiddleName = editProfile.MiddleName,
                };

                await _profileService.EditAsync(profile);
                return RedirectToAction("Profile");
            }
            ViewBag.ErrorMessage = "Пользователь не найден.";
            ViewBag.ErrorTitle = "Ошибка";
            return View("~/Views/Error/Error.cshtml");
        }
    }
}
