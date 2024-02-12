using InternalPortal.Infrastructure.LDAP.Interfaces;
using InternalPortal.Infrastructure.LDAP.Model;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text;

namespace InternalPortal.Infrastructure.LDAP.Services
{
    /// <inheritdoc cref="ILDAPUserService"/>
    public class LDAPUserService : ILDAPUserService
    {
        /// <inheritdoc cref="ILDAPUserService"/>
        public bool AuthenticateUserAsync(string domain, string userName, string password)
        {
            bool ret;
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://" + domain, userName, password);
                DirectorySearcher dsearch = new DirectorySearcher(de);
                SearchResult results = null;

                results = dsearch.FindOne();

                ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public async Task<User> GetUserAsync(string userName, string domain, string techUser, string techPassword)
        {
            DirectoryEntry de = new("LDAP://" + domain, techUser, techPassword);           
            DirectorySearcher ds = BuildUserSearcher(de);

            ds.Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + userName + "))";
            SearchResult sr = ds.FindOne();

            User user = new();
            if (sr != null)
            {
                user.Name = await GetPropertyValueAsync(sr, "name");
                user.Mail = await GetPropertyValueAsync(sr, "mail");
                user.GiveName = await GetPropertyValueAsync(sr, "givenname");
                user.Sn = await GetPropertyValueAsync(sr, "sn");
                user.Login = await GetPropertyValueAsync(sr, "userPrincipalName");
                user.DistinguishedName = await GetPropertyValueAsync(sr, "distinguishedName");
                byte[] userSIdArray = (byte[])sr.Properties["objectSid"][0];
                if (userSIdArray != null)
                {
                    user.Sid = new SecurityIdentifier(userSIdArray, 0).Value;
                } 
                
                var groups = sr.Properties["memberOf"];
                List<string> groupNames = new();
                foreach (var group in groups) {
                    if (group != null)
                    {
                        groupNames.Add(group.ToString().ToLower());
                    }
                    user.memberOf = groupNames;
                }
            }
            return user;
        }

        private DirectorySearcher BuildUserSearcher(DirectoryEntry de)
        {
            DirectorySearcher ds = new(de);

            ds.PropertiesToLoad.Add("name");

            ds.PropertiesToLoad.Add("mail");

            ds.PropertiesToLoad.Add("givenname");

            ds.PropertiesToLoad.Add("sn");

            ds.PropertiesToLoad.Add("userPrincipalName");

            ds.PropertiesToLoad.Add("distinguishedName");

            ds.PropertiesToLoad.Add("objectSid");

            ds.PropertiesToLoad.Add("memberOf");

            return ds;
        }

        private async Task<string> BuildOctetString(SecurityIdentifier sid)
        {
            byte[] items = new byte[sid.BinaryLength];
            sid.GetBinaryForm(items, 0);
            StringBuilder sb = new StringBuilder();
            await Task.Run(() =>
            {
                foreach (byte b in items)
                {
                    sb.Append(b.ToString("X2"));
                }
            });
            return sb.ToString();
        }

        private async Task<string> GetPropertyValueAsync(SearchResult sr, string propertyName)
        {
            StringBuilder sb = new();

            await Task.Run(() =>
            {
                if (sr.Properties[propertyName].Count > 0)
                    sb.Append(sr.Properties[propertyName][0].ToString());
            });
            return sb.ToString();
        }
    }
}
