namespace InternalPortal.Core.Models
{
    /// <summary>
    /// Data model file.
    /// </summary>
    public class UploadFile
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
    }
}
