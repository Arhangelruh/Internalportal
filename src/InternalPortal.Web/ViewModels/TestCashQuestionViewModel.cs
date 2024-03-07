namespace InternalPortal.Web.ViewModels
{
    public class TestCashQuestionViewModel
    {
        /// <summary>
        /// Question id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Question text.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Topic identifier.
        /// </summary>
        public int TestTopicId { get; set; }

        /// <summary>
        /// Answers list.
        /// </summary>
        public List<TestCashAnswerViewModel> Answers { get; set; }
    }
}
