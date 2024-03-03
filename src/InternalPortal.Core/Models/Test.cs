namespace InternalPortal.Core.Models
{
    /// <summary>
    /// Data model Test.
    /// </summary>
    public class Test
    {
        /// <summary>
        /// Test Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Start test Time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End test Time.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Navigation to Tests Answers.
        /// </summary>
        public ICollection<TestsAnswers> TestsAnswers { get; set; }

        /// <summary>
        /// Profile identifier.
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// Navigation to Profile.
        /// </summary>
        public Profile Profile { get; set; }
    }
}
