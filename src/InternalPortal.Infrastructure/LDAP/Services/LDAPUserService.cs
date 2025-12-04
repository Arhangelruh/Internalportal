using InternalPortal.Infrastructure.LDAP.Interfaces;
using InternalPortal.Infrastructure.LDAP.Model;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Principal;

namespace InternalPortal.Infrastructure.LDAP.Services
{
	/// <inheritdoc cref="ILDAPUserService"/>
	public class LDAPUserService : ILDAPUserService
	{
		public bool AuthenticateUser(string ldapServer, string domainFqdn, string userName, string password)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
					return false;

				string normalizedUser = NormalizeUserName(userName, domainFqdn);
				string escapedUser = EscapeLdap(normalizedUser);

				using var connection = new LdapConnection(ldapServer)
				{
					AuthType = AuthType.Basic,
					Timeout = TimeSpan.FromSeconds(10)
				};

				connection.SessionOptions.ProtocolVersion = 3;
				connection.Bind(new NetworkCredential(escapedUser, password));

				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<User?> GetUserAsync(string userName, string ldapServer, string techUser, string techPassword)
		{
			string escapedUser = EscapeLdap(userName);

			string[] attributes =
			{
				"name",
				"mail",
				"givenName",
				"sn",
				"userPrincipalName",
				"distinguishedName",
				"objectSid",
				"memberOf"
			};

			using var connection = new LdapConnection(ldapServer)
			{
				AuthType = AuthType.Basic,
				Timeout = new TimeSpan(0, 0, 30)
			};

			connection.SessionOptions.ProtocolVersion = 3;
			connection.Bind(new NetworkCredential(techUser, techPassword));

			string searchBase = GetDefaultNamingContext(connection);

			string filter = $"(&(objectCategory=person)(objectClass=user)(sAMAccountName={escapedUser}))";

			var request = new SearchRequest(searchBase, filter, SearchScope.Subtree, attributes);

			var response = (SearchResponse)connection.SendRequest(request);

			if (response.Entries.Count == 0)
				return null;

			var entry = response.Entries[0];
			var user = new User
			{
				Name = entry.Get("name"),
				Mail = entry.Get("mail"),
				GiveName = entry.Get("givenName"),
				Sn = entry.Get("sn"),
				Login = entry.Get("userPrincipalName"),
				DistinguishedName = entry.Get("distinguishedName"),
				memberOf = entry.GetList("memberOf").ConvertAll(s => s.ToLower())
			};

			var sidBytes = entry.GetBytes("objectSid");
			if (sidBytes != null)
				user.Sid = new SecurityIdentifier(sidBytes, 0).Value;

			return user;
		}

		private static string EscapeLdap(string value)
		{
			return value
				.Replace("\\", "\\5c")
				.Replace("*", "\\2a")
				.Replace("(", "\\28")
				.Replace(")", "\\29")
				.Replace("\0", "\\00")
				.Replace("/", "\\2f");
		}

		private static string NormalizeUserName(string userName, string domainFqdn)
		{
			if (userName.Contains("@"))
				return userName;

			if (userName.Contains("\\"))
			{
				var parts = userName.Split('\\');
				return $"{parts[1]}@{domainFqdn}";
			}

			return $"{userName}@{domainFqdn}";
		}

		private static string GetDefaultNamingContext(LdapConnection connection)
		{
			var request = new SearchRequest(
				"",
				"(objectClass=*)",
				SearchScope.Base,
				"defaultNamingContext"
			);

			var response = (SearchResponse)connection.SendRequest(request);

			return response.Entries[0].Attributes["defaultNamingContext"][0].ToString();
		}
	}
}
