using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;


namespace InternalPortal.Core.Services
{
    /// <inheritdoc cref="IProfileService"/>
    public class ProfileService : IProfileService
    {
        private readonly IRepository<Profile> _repository;
        public ProfileService(IRepository<Profile> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AddAsync(Profile profile)
        {
            if (profile is null)
            {
                throw new ArgumentNullException(nameof(profile));
            }
            
            await _repository.AddAsync(profile);
            await _repository.SaveChangesAsync();
        }

        public Task EditAsync(Profile profile)
        {
            throw new NotImplementedException();
        }

        public Task<Profile> GetProfileByIdAsync(int profileId)
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> GetProfileByUserSIDAsync(string userSID)
        {
            if (userSID is null)
            {
                throw new ArgumentNullException(nameof(userSID));
            }

            var profile = await _repository.GetEntityAsync(profile => profile.UserSid == userSID);

            if (profile is null)
            {
                return null;
            }
            
            return profile;
        }

    }
}

