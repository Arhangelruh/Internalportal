using InternalPortal.Infrastructure.LDAP.Constants;
using InternalPortal.Infrastructure.LDAP.Interfaces;
using InternalPortal.Infrastructure.LDAP.Model;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace InternalPortal.Infrastructure.LDAP.Services
{
	/// <inheritdoc cref="ILDAPUserService"/>
	public class LDAPUserService : ILDAPUserService
	{
		public LdapAuthResult AuthenticateUser(string ldapServer, string domainFqdn, string userName, string password, string techUser, string techPassword)
		{
			try
			{
				string normalizedUser = NormalizeUserName(userName, domainFqdn);
				string escapedUser = EscapeLdap(normalizedUser);

				using var connection = new LdapConnection(ldapServer)
				{
					AuthType = AuthType.Basic,
					Timeout = TimeSpan.FromSeconds(30)
				};

				connection.SessionOptions.ProtocolVersion = 3;

				try
				{
					string techNormalized = NormalizeUserName(techUser, domainFqdn);
					connection.Bind(new NetworkCredential(techNormalized, techPassword));
				}
				catch (LdapException ex)
				{
					Console.WriteLine("Tech bind failed: " + ex.ServerErrorMessage);
					return LdapAuthResult.UnknownError;
				}

				string searchBase = GetDefaultNamingContext(connection);

				string filter = $"(&(objectCategory=person)(objectClass=user)(sAMAccountName={userName}))";
				var searchRequest = new SearchRequest(searchBase, filter, SearchScope.Subtree, "distinguishedName", "userAccountControl");

				var searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

				if (searchResponse.Entries.Count == 0)
					return LdapAuthResult.UserNotFound;

				var entry = searchResponse.Entries[0];

				string dn = entry.DistinguishedName;

				if (entry.Attributes.Contains("userAccountControl"))
				{
					int uac = GetUserAccountControl(entry);

					if ((uac & 0x0002) != 0)
						return LdapAuthResult.AccountDisabled;
				}

				try
				{
					connection.Bind(new NetworkCredential(normalizedUser, password));
					return LdapAuthResult.Success;
				}
				catch (LdapException ex)
				{
					return MapLdapBindError(ex);
				}
			}
			catch
			{
				return LdapAuthResult.UnknownError;
			}
		}

		public User? GetUser(string userName, string ldapServer, string techUser, string techPassword)
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
			if (userName.Contains('@'))
				return userName;

			if (userName.Contains('\\'))
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

		private static LdapAuthResult MapLdapBindError(LdapException ex)
		{
			string msg = ex.ServerErrorMessage ?? "";

			return msg switch
			{
				var s when s.Contains("data 525") => LdapAuthResult.UserNotFound,                  
				var s when s.Contains("data 52e") => LdapAuthResult.InvalidPassword,               
				var s when s.Contains("data 532") => LdapAuthResult.PasswordExpired,               
				var s when s.Contains("data 533") => LdapAuthResult.AccountDisabled,               
				var s when s.Contains("data 701") => LdapAuthResult.AccountExpired,                
				var s when s.Contains("data 773") => LdapAuthResult.MustChangePassword,            
				var s when s.Contains("data 775") => LdapAuthResult.AccountLocked,                 
				var s when s.Contains("data 530") => LdapAuthResult.LoginNotAllowedAtThisTime,     
				var s when s.Contains("data 531") => LdapAuthResult.LoginNotAllowedFromWorkstation,
				_ => LdapAuthResult.UnknownError
			};
		}

		private static int GetUserAccountControl(SearchResultEntry entry)
		{
			var value = entry.Attributes["userAccountControl"][0];

			return value switch
			{
				int i => i,
				string s => int.Parse(s),
				byte[] bytes => int.Parse(Encoding.UTF8.GetString(bytes)),
				_ => throw new InvalidCastException(
					$"Unsupported userAccountControl type: {value.GetType()}"
				)
			};
		}
	}
}
