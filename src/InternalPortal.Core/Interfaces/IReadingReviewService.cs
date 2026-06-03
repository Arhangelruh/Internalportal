using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
	public interface IReadingReviewService
	{
		/// <summary>
		/// Add record.
		/// </summary>
		/// <returns></returns>
		Task AddRecordAsync(ReadingReview record);

		/// <summary>
		/// Get record by document.
		/// </summary>
		/// <param name="fileId"></param>
		/// <returns></returns>
		Task<List<ReadingReview>> GetRecordsByDocumentAsync(int fileId);

		/// <summary>
		/// Delete records by user.
		/// </summary>
		/// <param name="profileId"></param>
		/// <returns></returns>
		Task DeleteRecordsByUserAsync(int profileId);

		/// <summary>
		/// Delete records by file.
		/// </summary>
		/// <param name="fileId"></param>
		/// <returns></returns>
		Task DeleteRecordsByFileAsync(int fileId);
	}
}
