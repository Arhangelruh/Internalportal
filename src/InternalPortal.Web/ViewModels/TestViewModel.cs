using InternalPortal.Core.Models;

namespace InternalPortal.Web.ViewModels
{
    public class TestViewModel
    {
        /// <summary>
        /// Test Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// start date.
        /// </summary>
        public DateTime startDate { get; set; }

        /// <summary>
        /// List questions.
        /// </summary>
        public IList<TestCashQuestionViewModel> CashQuestions { get; set; }

    }
}
