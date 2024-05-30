namespace InternalPortal.Web.ViewModels
{
    public class UploadFileViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Untrasted file name.
        /// </summary>
        public string UntrastedName { get; set; }

        /// <summary>
        /// Trusted file name.
        /// </summary>
        public string TrustedName { get; set; }

        /// <summary>
        /// Extension string.
        /// </summary>
        public string Extension { get; set; }
    }
}
