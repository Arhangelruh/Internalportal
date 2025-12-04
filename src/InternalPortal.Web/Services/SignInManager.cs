using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Infrastructure.LDAP.Interfaces;
using InternalPortal.Web.Constants;
using InternalPortal.Web.Interfaces;
using InternalPortal.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InternalPortal.Web.Services
{
	public class SignInManager(IOptions<ConfigurationAD> configurationAD,
						 ILDAPUserService lDAPUserService,
						 IHttpContextAccessor httpContextAccessor,
						 IProfileService profileService
							 ) : ISignInManager
	{
		private readonly ConfigurationAD _configurationAD = configurationAD.Value ?? throw new ArgumentNullException(nameof(configurationAD.Value));
		private readonly ILDAPUserService _lDAPUserService = lDAPUserService ?? throw new ArgumentNullException(nameof(lDAPUserService));
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		private readonly IProfileService _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));

		public async Task<bool> SignIn(string userName, string password)
		{

			var checkUser = _lDAPUserService.AuthenticateUser(_configurationAD.LDAPserver, _configurationAD.DomainFqdn, userName, password);

			if (checkUser)
			{
				var user = await _lDAPUserService.GetUserAsync(userName, _configurationAD.LDAPserver, _configurationAD.Username, _configurationAD.Password);
				if (user != null)
				{
					var checkProfileBySid = await _profileService.GetProfileByUserSIDAsync(user.Sid);
					if (checkProfileBySid == null)
					{
						await _profileService.AddAsync(new Profile { LastName = user.Sn, Name = user.Name, UserSid = user.Sid });
					}

					var claims = new List<Claim>
					{
					new(ClaimTypes.WindowsAccountName, user.Login),
					new(ClaimTypes.Name, user.Name),
					new(ClaimTypes.Email, user.Mail),
					new(ClaimTypes.Sid, user.Sid)
					};

					foreach (var group in user.memberOf)
					{
						if (group.Contains(_configurationAD.Managers.ToLower()))
							claims.Add(new Claim(ClaimTypes.Role, UserConstants.ManagerRole));
						if (group.Contains(_configurationAD.CashStudents.ToLower()))
							claims.Add(new Claim(ClaimTypes.Role, UserConstants.CashStudents));

					}

					var identity = new ClaimsIdentity(
					   claims,
					   "LDAP",
					   ClaimTypes.Name,
					   ClaimTypes.Role);

					var principal = new ClaimsPrincipal(identity);

					if (_httpContextAccessor.HttpContext != null)
					{
						try
						{
							await _httpContextAccessor.HttpContext.SignInAsync(
								CookieAuthenticationDefaults.AuthenticationScheme,
								principal
							);
							return true;
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Signing in has failed. {ex.Message}");
						}
					}
					return true;
				}
			}
			return false;
		}


		public async Task SignOut()
		{
			if (_httpContextAccessor.HttpContext != null)
			{
				await _httpContextAccessor.HttpContext.SignOutAsync(
					CookieAuthenticationDefaults.AuthenticationScheme
				);
			}
			else
			{
				throw new Exception(
					"For some reasons, HTTP context is null, signing out cannot be performed"
				);
			}
		}
	}
}
