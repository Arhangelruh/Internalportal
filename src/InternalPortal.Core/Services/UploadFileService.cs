using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Core.Services
{
    public class UploadFileService(IRepository<UploadFile> repository, IReadingReviewService readingReview) : IUploadFileService
    {
        private readonly IRepository<UploadFile> _repository = repository ?? throw new ArgumentNullException(nameof(repository));	
        private readonly IReadingReviewService _readinReview = readingReview ?? throw new ArgumentNullException(nameof(readingReview));

        public async Task AddAsync(UploadFile uploadFile) {
           
            ArgumentNullException.ThrowIfNull(uploadFile);

            await _repository.AddAsync(uploadFile);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int fileId)
        {
            var file = await _repository.GetEntityAsync(topic => topic.Id.Equals(fileId));

            _repository.Delete(file);
            await _repository.SaveChangesAsync();

            await _readinReview.DeleteRecordsByFileAsync(fileId);
        }

        public async Task<UploadFile> GetFileByIdAsync(int fileId)
        {
            var file = await _repository.GetEntityAsync(file => file.Id == fileId);

            if (file is null)
            {
                return null;
            }

            return file;
        }

        public async Task<List<UploadFile>> GetAllFilesAsync() {
            return await _repository
                   .GetAll()
                   .AsNoTracking()
                   .ToListAsync();
        }

        public async Task<UploadFile> GetFileByGuidAsync(string trustName)
        {
            var file = await _repository.GetEntityAsync(file => file.TrustedName == trustName);

            if (file is null)
            {
                return null;
            }

            return file;
        }
    }
}
