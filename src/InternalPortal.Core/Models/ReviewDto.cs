namespace InternalPortal.Core.Models
{
	public class ReviewDto
	{
		/// <summary>
		/// Review record id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// File name.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// User Name.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Middle name.
		/// </summary>
		public string? MiddleName { get; set; }

		/// <summary>
		/// Time when user confirm reading the file.
		/// </summary>
		public DateTime ConfirmTime { get; set; }
	}
}
