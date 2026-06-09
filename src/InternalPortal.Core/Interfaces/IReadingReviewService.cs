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
		/// <returns>List records</returns>
		Task<List<ReviewDto>> GetRecordsByDocumentAsync(int fileId);

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

		/// <summary>
		/// Get record by file and profile id.
		/// </summary>
		/// <param name="fileId">file id</param>
		/// <param name="profileId">profile id</param>
		/// <returns>Reading review record</returns>
		Task<ReadingReview>? CheckRecordAsync(int fileId, int profileId);
	}
}
