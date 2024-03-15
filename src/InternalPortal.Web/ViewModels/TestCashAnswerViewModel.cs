namespace InternalPortal.Web.ViewModels
{
    public class TestCashAnswerViewModel
    {
        /// <summary>
        /// Answer Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Answer text.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Answer Meaning.
        /// </summary>
        public bool Meaning { get; set; }

        /// <summary>
        /// User choise.
        /// </summary>
        public bool Choise { get; set; }
    }
}
