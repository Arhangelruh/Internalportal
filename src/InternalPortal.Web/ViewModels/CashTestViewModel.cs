using InternalPortal.Infrastucture.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace InternalPortal.Web.ViewModels
{
    public class CashTestViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Topic text.
        /// </summary>
        [Required(ErrorMessage = "Тема не может быть пустой.")]
        [StringLength(ConfigurationConstants.SqlMaxLengthShort, MinimumLength = 1, ErrorMessage = "Тема должна содержать от {2} до {1} символов.")]
        public string CashTestName { get; set; }

        /// <summary>
        /// Amount questions for tests.
        /// </summary>
        public int TestQuestionsAmount { get; set; }

        /// <summary>
        /// Amount wrong answers for tests.
        /// </summary>
        public int WrongAnswersAmount { get; set; }

        /// <summary>
        /// Acual topic.
        /// </summary>
        public bool IsActual { get; set; }
    }
}
