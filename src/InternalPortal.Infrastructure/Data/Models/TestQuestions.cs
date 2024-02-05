namespace InternalPortal.Infrastucture.Data.Models
{
    /// <summary>
    /// Data model Test Question.
    /// </summary>
    public class TestQuestions
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Question.
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// Topic identifier.
        /// </summary>
        public int TestTopicId { get; set; }

        /// <summary>
        /// Navigation to TestTopics.
        /// </summary>
        public TestTopics TestTopic { get; set; }

        /// <summary>
        /// Navigation to question answers.
        /// </summary>
        public ICollection<TestQuestionAnswers> TestAnswers { get; set; }

    }
}
