namespace InternalPortal.Core.Models
{
    /// <summary>
    /// Data model TestTopics.
    /// </summary>
    public class TestTopics
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Topic text.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Navigation to questions.
        /// </summary>
        public ICollection<TestQuestions> TestQuestions { get; set; }

        /// <summary>
        /// Navigation to Tests.
        /// </summary>
        public ICollection<Test> Tests { get; set; }
    }
}
