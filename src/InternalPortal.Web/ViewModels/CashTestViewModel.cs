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
        public string CashTestName { get; set; }

        /// <summary>
        /// Acual topic.
        /// </summary>
        public bool IsActual { get; set; }
    }
}
