using InternalPortal.Core.Models;
using InternalPortal.Infrastucture.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastructure.Data.Configurations
{
	public class ReadingReviewConfiguration : IEntityTypeConfiguration<ReadingReview>
	{
		///<inheritdoc/>
		public void Configure(EntityTypeBuilder<ReadingReview> builder)
		{
			builder = builder ?? throw new ArgumentNullException(nameof(builder));

			builder.ToTable(TableConstants.ReadingReview, SchemaConstants.File)
				.HasKey(c => c.Id);

			builder.HasOne(rr=>rr.Profile)
				.WithMany(profile=>profile.ReadingReviews)
				.HasForeignKey(rr=>rr.ProfileId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(rr => rr.UploadFile)
				.WithMany(file => file.ReadingReviews)
				.HasForeignKey(rr => rr.FileId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
