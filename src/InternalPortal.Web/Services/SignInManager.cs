using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Infrastructure.LDAP.Constants;
using InternalPortal.Infrastructure.LDAP.Interfaces;
using InternalPortal.Web.Constants;
using InternalPortal.Web.Interfaces;
using InternalPortal.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InternalPortal.Web.Services
{
	/// <inheritdoc cref="ILDAPUserService"/>
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

		public async Task<bool> SignIn(string userName, string password, ModelStateDictionary modelState)
		{
			var checkUser = _lDAPUserService.AuthenticateUser(_configurationAD.LDAPserver, _configurationAD.DomainFqdn, userName, password, _configurationAD.Username, _configurationAD.Password);

			if (checkUser == LdapAuthResult.Success)
			{
				var user = _lDAPUserService.GetUser(userName, _configurationAD.LDAPserver, _configurationAD.Username, _configurationAD.Password);
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
				return false;
			}
			else
			{				
				switch (checkUser)
				{
					case LdapAuthResult.UserNotFound:
						modelState.AddModelError("",$"Пользователь {userName} не найден.");
						break;
					case LdapAuthResult.InvalidPassword:
						modelState.AddModelError("", "Не верно указан пароль.");
						break;
					case LdapAuthResult.PasswordExpired:
						modelState.AddModelError("", "Истек срок действия пароля.");
						break;
					case LdapAuthResult.AccountDisabled:
						modelState.AddModelError("", "Учетная запись отключена.");
						break;
					case LdapAuthResult.AccountExpired:
						modelState.AddModelError("", "Истек срок действия учетной записи.");
						break;
					case LdapAuthResult.MustChangePassword:
						modelState.AddModelError("", "Требуется смена пароля.");
						break;
					case LdapAuthResult.AccountLocked:
						modelState.AddModelError("", "Учетная запись заблокирована.");
						break;
					case LdapAuthResult.LoginNotAllowedAtThisTime:
						modelState.AddModelError("", "Запрещена работа вашей учетной записи в данное время.");
						break;
					case LdapAuthResult.LoginNotAllowedFromWorkstation:
						modelState.AddModelError("", "Запрещена работа вашей учетной записи с этой машины.");
						break;
					default:
						modelState.AddModelError("", "Неизвестная ошибка.");
						break;
				}
				return false;
			}			
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
