namespace InternalPortal.Core.Models
{
    /// <summary>
    /// Data model Cash Test.
    /// </summary>
    public class CashTest
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Cash text.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Acual cash test.
        /// </summary>
        public bool IsActual { get; set; }


        /// <summary>
        /// Navigation to topics.
        /// </summary>
        public ICollection<TestTopics> TestTopics { get; set; }
    }
}
