using System.ComponentModel.DataAnnotations;

namespace InternalPortal.Web.ViewModels
{
    public class TestQuestionViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Question.
        /// </summary>
        [Required(ErrorMessage = "Вопрос не может быть пустой.")]
        public string QuestionText { get; set; }

        /// <summary>
        /// Topic identifier.
        /// </summary>
        public int TestTopicId { get; set; }

        /// <summary>
        /// Is Actual question or no.
        /// </summary>
        public bool IsActual { get; set; }
    }
}
