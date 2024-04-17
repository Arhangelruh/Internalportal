using InternalPortal.Core.Models;
using InternalPortal.Infrastucture.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastructure.Data.Configurations
{
    /// <summary>
    /// EF Configuration for file.
    /// </summary>
    public class UploadFileConfiguration : IEntityTypeConfiguration<UploadFile>
    {
        public void Configure(EntityTypeBuilder<UploadFile> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.UploadFiles, SchemaConstants.File)
                .HasKey(file => file.Id);

            builder.Property(file => file.UntrastedName)
                .IsRequired();

            builder.Property(file => file.TrustedName)
                .IsRequired();
        }
    }
}
