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
        /// Start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Test name.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Test result.
        /// </summary>
        public bool TestResult { get; set; }
    }
}
