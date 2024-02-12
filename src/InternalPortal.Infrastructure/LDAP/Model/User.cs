namespace InternalPortal.Infrastructure.LDAP.Model
{
    public class User
    {
        /// <summary>
        /// Full Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email Address.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// First Name.
        /// </summary>
        public string GiveName { get; set; }

        /// <summary>
        /// Last Name.
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// UserPrincipalName Name.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Distinguished Name.
        /// </summary>
        public string DistinguishedName { get; set; }

        /// <summary>
        /// User Sid.
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// User groups.
        /// </summary>
        public List<string> memberOf { get; set; }
    }
}
