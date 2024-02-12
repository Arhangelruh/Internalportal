namespace InternalPortal.Core.Models
{
    /// <summary>
    /// Data model answer.
    /// </summary>
    public class TestQuestionAnswers
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Answer.
        /// </summary>
        public string AnswerText { get; set; }

        /// <summary>
        /// Meaning answer true or false.
        /// </summary>
        public bool Meaning { get; set; }

        /// <summary>
        /// Question identifier.
        /// </summary>
        public int TestQuestionId { get; set; }

        /// <summary>
        /// Navigation to test question.
        /// </summary>
        public TestQuestions TestQuestion { get; set; }

        /// <summary>
        /// Navigation to test answers.
        /// </summary>
        public ICollection<TestsAnswers> TestsAnswers { get; set; }
    }
}
