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
        /// Acual topic.
        /// </summary>
        public bool IsActual { get; set; }

        /// <summary>
        /// Test identifier.
        /// </summary>
        public int CashTestId { get; set; }

        /// <summary>
        /// Navigation to Cash Test.
        /// </summary>
        public CashTest CashTest { get; set; }

        /// <summary>
        /// Navigation to questions.
        /// </summary>
        public ICollection<TestQuestions> TestQuestions { get; set; }
    }
}
