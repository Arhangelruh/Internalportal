namespace InternalPortal.Web.ViewModels
{
    public class ProfileViewModel
    {
        /// <summary>
        /// First name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Middle name.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// User SID.
        /// </summary>
        public string Sid { get; set; }
    }
}
