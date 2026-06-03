namespace InternalPortal.Core.Models
{
	public class ReadingReview
	{
		/// <summary>
		/// Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Review time.
		/// </summary>
		public DateTime ReviewTime { get; set; }

		/// <summary>
		/// Profile identifier.
		/// </summary>
		public int ProfileId { get; set; }

		/// <summary>
		/// Navigate to profile.
		/// </summary>
		public Profile Profile { get; set; } = null!;

		/// <summary>
		/// File idintifier.
		/// </summary>
		public int FileId { get; set; }

		/// <summary>
		/// Navigate to uplodaded files.
		/// </summary>
		public UploadFile UploadFile { get; set; } = null!;
	}
}
