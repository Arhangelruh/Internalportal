using InternalPortal.Infrastructure.LDAP.Constants;
using InternalPortal.Infrastructure.LDAP.Model;

namespace InternalPortal.Infrastructure.LDAP.Interfaces
{
    public interface ILDAPUserService
    {
		/// <summary>
		/// Authenticate user.
		/// </summary>
		/// <param name="domain">LDAP string from configuration.</param>
		/// <param name="userName">Login</param>
		/// <param name="password">Password</param>
		/// <returns>result</returns>
		LdapAuthResult AuthenticateUser(string ldapServer, string domainFqdn, string userName, string password, string techUser, string techPassword);

        /// <summary>
        /// Get user info from LDAP.
        /// </summary>
        /// <param name="userName">Login</param>
        /// <param name="domain">LDAP string from configuration</param>
        /// <param name="techUser">Technical user login for get data from LDAP</param>
        /// <param name="techPassword">Technical user password for get data from LDAP</param>
        /// <returns>User model</returns>
        User? GetUser(string userName, string domain, string techUser, string techPassword);
    }
}
