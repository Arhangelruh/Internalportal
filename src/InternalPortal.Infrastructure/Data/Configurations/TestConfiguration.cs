using InternalPortal.Infrastucture.Data.Constants;
using InternalPortal.Infrastucture.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastucture.Data.Configurations
{
    /// <summary>
    /// EF Configuration for Test.
    /// </summary>
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Tests, SchemaConstants.Test)
                .HasKey(test => test.Id);

            builder.HasOne(testtopic => testtopic.TestTopic)
                .WithMany(test => test.Tests)
                .HasForeignKey(testtopic => testtopic.TestTopicId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(test => test.Profile)
                .WithMany(tests => tests.Tests)
                .HasForeignKey(test => test.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
