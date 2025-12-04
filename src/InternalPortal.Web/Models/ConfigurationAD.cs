namespace InternalPortal.Web.Models
{
	public class ConfigurationAD
	{
		/// <summary>
		/// Domain port.
		/// </summary>
		public int Port { get; set; } = 389;

		/// <summary>
		/// FQDN address.
		/// </summary>
		public string DomainFqdn { get; set; } = string.Empty;

		/// <summary>
		/// Domain address.
		/// </summary>
		public string LDAPserver { get; set; } = string.Empty;

		/// <summary>
		/// User name for connecting to domain.
		/// </summary>
		public string Username { get; set; } = string.Empty;

		/// <summary>
		/// Password for connecting to domain.
		/// </summary>
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// Query Base.
		/// </summary>
		public string LDAPQueryBase { get; set; } = string.Empty;

		/// <summary>
		/// Managers group.
		/// </summary>
		public string Managers { get; set; } = string.Empty;

		/// <summary>
		/// Group for people who take cash test.
		/// </summary>
		public string CashStudents { get; set; } = string.Empty;
	}
}
