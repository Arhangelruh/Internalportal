namespace InternalPortal.Core.Models
{
    /// <summary>
    /// Data model Cash Test.
    /// </summary>
    public class CashTest
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Cash text.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Acual cash test.
        /// </summary>
        public bool IsActual { get; set; }

        /// <summary>
        /// Amounth questions in test.
        /// </summary>
        public int TestQuestions { get; set; }

        /// <summary>
        /// Amounth wrong answers.
        /// </summary>
        public int WrongAnswers { get; set; }

        /// <summary>
        /// Navigation to topics.
        /// </summary>
        public ICollection<TestTopics> TestTopics { get; set; }

        /// <summary>
        /// Navigation to test.
        /// </summary>
        public ICollection<Test> Tests { get; set; }
    }
}
