namespace InternalPortal.Core.Models
{
    public class TestScore
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Profile identifier.
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// Navigation to Profile.
        /// </summary>
        public Profile Profile { get; set; }
    }
}
