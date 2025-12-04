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
        bool AuthenticateUser(string ldapServer, string domainFqdn, string userName, string password);

        /// <summary>
        /// Get user info from LDAP.
        /// </summary>
        /// <param name="userName">Login</param>
        /// <param name="domain">LDAP string from configuration</param>
        /// <param name="techUser">Technical user login for get data from LDAP</param>
        /// <param name="techPassword">Technical user password for get data from LDAP</param>
        /// <returns>User model</returns>
        Task<User?> GetUserAsync(string userName, string domain, string techUser, string techPassword);
    }
}
