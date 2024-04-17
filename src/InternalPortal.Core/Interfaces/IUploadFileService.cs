using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
    public interface IUploadFileService
    {
        /// <summary>
        /// Add file data.
        /// </summary>
        /// <param name="uploadFile">UpdateFile model</param>
        /// <returns></returns>
        Task AddAsync(UploadFile uploadFile);

        /// <summary>
        /// Delete file data.
        /// </summary>
        /// <param name="fileId">Upload file id</param>
        /// <returns></returns>
        Task DeleteAsync(int fileId);

        /// <summary>
        /// Get file data by id.
        /// </summary>
        /// <param name="fileId">File id.</param>
        /// <returns>UploadFile data model</returns>
        Task<UploadFile> GetFileByIdAsync(int fileId);

        /// <summary>
        /// Get all files.
        /// </summary>
        /// <returns>List of upload data</returns>
        Task<List<UploadFile>> GetAllFilesAsync();

        /// <summary>
        /// Get file data by trustName.
        /// </summary>
        /// <param name="trustName">Trust name.</param>
        /// <returns>UploadFile data model.</returns>
        Task<UploadFile> GetFileByGuidAsync(string trustName);
    }
}
