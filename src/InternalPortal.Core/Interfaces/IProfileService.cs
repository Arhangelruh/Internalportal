using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
    /// <summary>
    /// Service for working with profile data.
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Add profile from user identity.
        /// </summary>
        /// <param name="profile">Dto model</param>
        Task AddAsync(Profile profile);

        /// <summary>
        /// Edit profile.
        /// </summary>
        /// <param name="profile">Dto model</param>
        Task EditAsync(Profile profile);

        /// <summary>
        /// Get profile from base.
        /// </summary>
        /// <param name="userId">Search profil by UserId key</param>
        Task<Profile> GetProfileByUserSIDAsync(string userSID);

        /// <summary>
        /// Get profile from base by id.
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns>profiledto</returns>
        Task<Profile> GetProfileByIdAsync(int profileId);
    }
}
