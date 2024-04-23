namespace InternalPortal.Web.Models
{
    public class ConfigurationFiles
    {
        /// <summary>
        /// Cash files
        /// </summary>
        public string Files { get; set; } = string.Empty;

        public long FileSizeLimit { get; set; } = 10485760;
    }
}
