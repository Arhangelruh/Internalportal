namespace InternalPortal.Infrastructure.LDAP.Constants
{
	/// <summary>
	/// Static list of users statuses in AD.
	/// </summary>
	public enum LdapAuthResult
	{
		Success,
		UserNotFound,
		InvalidPassword,
		PasswordExpired,
		MustChangePassword,
		AccountLocked,
		AccountDisabled,
		AccountExpired,
		LoginNotAllowedAtThisTime,
		LoginNotAllowedFromWorkstation,
		UnknownError
	}
}
