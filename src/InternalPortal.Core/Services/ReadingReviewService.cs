using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Core.Services
{
	/// <inheritdoc cref="IReadingReviewService"/>
	public class ReadingReviewService(IRepository<ReadingReview> repository) : IReadingReviewService
	{
		private readonly IRepository<ReadingReview> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

		public async Task AddRecordAsync(ReadingReview record)
		{
			ArgumentNullException.ThrowIfNull(record);

			await _repository.AddAsync(record);
			await _repository.SaveChangesAsync();
		}

		public async Task DeleteRecordsByFileAsync(int fileId)
		{
			await _repository.DeleteWhereAsync(x=> x.FileId == fileId);
		}

		public async Task DeleteRecordsByUserAsync(int profileId)
		{
			await _repository.DeleteWhereAsync(x=> x.ProfileId == profileId);
		}

		public async Task<List<ReadingReview>> GetRecordsByDocumentAsync(int fileId)
		{
			return await _repository
			  .GetAll()
			  .Where(x => x.FileId == fileId)
			  .AsNoTracking()
			  .ToListAsync();
		}
	}
}
