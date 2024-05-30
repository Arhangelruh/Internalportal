using InternalPortal.Core.Models;
using InternalPortal.Infrastucture.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastructure.Data.Configurations
{
    /// <summary>
    /// EF Configuration for Test.
    /// </summary>
    internal class TestsScoreConfiguration : IEntityTypeConfiguration<TestScore>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<TestScore> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.TestsScore, SchemaConstants.Test)
                .HasKey(testscore => testscore.Id);

            builder.Property(testscore => testscore.Score)
                .IsRequired();

            builder.HasOne(testscore => testscore.Profile)
                .WithOne(profile => profile.TestScore)
                .HasForeignKey<TestScore>(testscore => testscore.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
