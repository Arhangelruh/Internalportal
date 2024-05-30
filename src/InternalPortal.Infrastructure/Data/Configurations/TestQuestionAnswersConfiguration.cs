using InternalPortal.Core.Models;
using InternalPortal.Infrastucture.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastucture.Data.Configurations
{
    /// <summary>
    /// EF Configuration for TestQuestionAnswers.
    /// </summary>
    public class TestQuestionAnswersConfiguration : IEntityTypeConfiguration<TestQuestionAnswers>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<TestQuestionAnswers> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.TestQuestionAnswers, SchemaConstants.Test)
                .HasKey(answer => answer.Id);

            builder.Property(answer => answer.AnswerText)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.SqlMaxLengthLong);

            builder.Property(answer => answer.Meaning)
                .IsRequired();

            builder.Property(answer => answer.IsActual)
                .IsRequired();

            builder.HasOne(testquestion => testquestion.TestQuestion)
                .WithMany(testanswer => testanswer.TestAnswers)
                .HasForeignKey(testquestion => testquestion.TestQuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
