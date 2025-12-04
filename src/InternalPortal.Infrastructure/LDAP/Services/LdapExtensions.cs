using System.DirectoryServices.Protocols;

namespace InternalPortal.Infrastructure.LDAP.Services
{
	public static class LdapExtensions
	{
		public static string? Get(this SearchResultEntry entry, string attribute)
		{
			if (!entry.Attributes.Contains(attribute))
				return null;

			var vals = entry.Attributes[attribute]?.GetValues(typeof(string));
			return vals is { Length: > 0 } ? vals[0] as string : null;
		}

		public static List<string> GetList(this SearchResultEntry entry, string attribute)
		{
			List<string> result = new();

			if (!entry.Attributes.Contains(attribute))
				return result;

			var vals = entry.Attributes[attribute]?.GetValues(typeof(string));
			if (vals == null) return result;

			foreach (var val in vals)
				if (val != null)
					result.Add(val.ToString()!);

			return result;
		}

		public static byte[]? GetBytes(this SearchResultEntry entry, string attribute)
		{
			if (!entry.Attributes.Contains(attribute))
				return null;

			var vals = entry.Attributes[attribute]?.GetValues(typeof(byte[]));
			if (vals == null || vals.Length == 0)
				return null;

			return (byte[])vals[0];
		}
	}

}
