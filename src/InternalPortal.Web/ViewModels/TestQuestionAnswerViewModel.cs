using System.ComponentModel.DataAnnotations;

namespace InternalPortal.Web.ViewModels
{
    public class TestQuestionAnswerViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Answer.
        /// </summary>
        [Required(ErrorMessage = "Ответ не может быть пустой.")]
        public string AnswerText { get; set; }

        /// <summary>
        /// Meaning answer true or false.
        /// </summary>
        public bool Meaning { get; set; } 

        /// <summary>
        /// Is Actual answer or no.
        /// </summary>
        public bool IsActual { get; set; }

        /// <summary>
        /// Question identifier.
        /// </summary>
        public int TestQuestionId { get; set; }

        /// <summary>
        /// Topic identifier.
        /// </summary>
        public int TestTopicId { get; set; }

        /// <summary>
        /// Test identifier.
        /// </summary>
        public int CashTestId { get; set; }
    }
}
