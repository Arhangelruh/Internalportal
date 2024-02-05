using InternalPortal.Infrastucture.Data.Constants;
using InternalPortal.Infrastucture.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastucture.Data.Configurations
{
    /// <summary>
    /// EF Configuration for TestTopics.
    /// </summary>
    public class TestTopicsConfiguration : IEntityTypeConfiguration<TestTopics>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<TestTopics> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.TestTopics, SchemaConstants.Test)
                .HasKey(topic => topic.Id);

            builder.Property(topic => topic.TopicName)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.SqlMaxLengthMedium);
        }
    }
}
