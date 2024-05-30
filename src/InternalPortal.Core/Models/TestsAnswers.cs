namespace InternalPortal.Core.Models
{
    /// <summary>
    /// Data model for connection table TestAnswers.
    /// </summary>
    public class TestsAnswers
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Answer identifier.
        /// </summary>
        public int AnswerId { get; set; }

        /// <summary>
        /// Navigation to Answers table.
        /// </summary>
        public TestQuestionAnswers TestQuestionAnswer { get; set; }

        /// <summary>
        /// Test identifier.
        /// </summary>
        public int TestId { get; set; }

        /// <summary>
        /// Navigation to Tests.
        /// </summary>
        public Test Test { get; set; }

        /// <summary>
        /// Answer status.
        /// </summary>
        public bool AnswerStatus { get; set; }
    }
}
