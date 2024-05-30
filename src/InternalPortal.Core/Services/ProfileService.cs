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
            ArgumentNullException.ThrowIfNull(profile);

            await _repository.AddAsync(profile);
            await _repository.SaveChangesAsync();
        }

        public async Task EditAsync(Profile profile)
        {
            ArgumentNullException.ThrowIfNull(profile);

            var editProfile = await _repository.GetEntityAsync(q => q.Id.Equals(profile.Id));
            editProfile.Name = profile.Name;
            editProfile.MiddleName = profile.MiddleName;
            editProfile.LastName = profile.LastName;
            _repository.Update(editProfile);
            await _repository.SaveChangesAsync();
        }

        public async Task<Profile> GetProfileByIdAsync(int profileId)
        {
            var profile = await _repository.GetEntityAsync(transactionModel => transactionModel.Id == profileId);

            if (profile is null)
            {
                return new Profile();
            }
          
            return profile;
        }

        public async Task<Profile> GetProfileByUserSIDAsync(string userSID)
        {
            ArgumentNullException.ThrowIfNull(userSID);

            var profile = await _repository.GetEntityAsync(profile => profile.UserSid == userSID);

            if (profile is null)
            {
                return null;
            }
            
            return profile;
        }

    }
}

