namespace InternalPortal.Core.Models
{
    /// <summary>
    /// Data model Profile.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Last Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Middle name.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Users Sid from LDAP.
        /// </summary>
        public string UserSid { get; set; }

        /// <summary>
        /// Navigation to tests.
        /// </summary>
        public ICollection<Test> Tests { get; set; }

        /// <summary>
        /// Navigation to TestScore.
        /// </summary>
        public TestScore TestScore { get; set; }
    }
}
