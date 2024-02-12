using InternalPortal.Core.Models;
using InternalPortal.Infrastucture.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastucture.Data.Configurations
{
    /// <summary>
    /// EF Configuration for Profile.
    /// </summary>
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Profiles, SchemaConstants.DBO)
                .HasKey(profile => profile.Id);

            builder.Property(profile => profile.Name)
                .HasMaxLength(ConfigurationConstants.SqlMaxLengthShort);

            builder.Property(profile => profile.LastName)
                .HasMaxLength(ConfigurationConstants.SqlMaxLengthMedium);

            builder.Property(profile => profile.MiddleName)
                .HasMaxLength(ConfigurationConstants.SqlMaxLengthShort);

            builder.Property(profile => profile.UserSid)
                .HasMaxLength(ConfigurationConstants.SqlMaxLengthShort);
        }
    }
}
