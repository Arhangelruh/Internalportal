namespace InternalPortal.Web.ViewModels
{
    public class TestResultViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int TestId {  get; set; }

        /// <summary>
        /// Profile.
        /// </summary>
        public ProfileViewModel Profile { get; set; }

        /// <summary>
        /// start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// end date.
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
