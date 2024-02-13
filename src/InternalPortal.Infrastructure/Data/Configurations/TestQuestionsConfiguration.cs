using InternalPortal.Core.Models;
using InternalPortal.Infrastucture.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastucture.Data.Configurations
{
    /// <summary>
    /// EF Configuration for TestQuestionAnswers.
    /// </summary>
    public class TestQuestionsConfiguration : IEntityTypeConfiguration<TestQuestions>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<TestQuestions> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.TestQuestions, SchemaConstants.Test)
                .HasKey(question => question.Id);

            builder.Property(question => question.QuestionText)
               .IsRequired()
               .HasMaxLength(ConfigurationConstants.SqlMaxLengthLong);

            builder.Property(question => question.IsActual)
                .IsRequired();

            builder.HasOne(testtopic => testtopic.TestTopic)
                .WithMany(testquestion => testquestion.TestQuestions)
                .HasForeignKey(testquestion => testquestion.TestTopicId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
