using InternalPortal.Infrastucture.Data.Constants;
using InternalPortal.Infrastucture.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastucture.Data.Configurations
{
    /// <summary>
    /// EF Configuration for Test.
    /// </summary>
    public class TestsAnswersConfiguration : IEntityTypeConfiguration<TestsAnswers>
    {
        ///<inheritdoc/>
        public void Configure(EntityTypeBuilder<TestsAnswers> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.TestsAnswers, SchemaConstants.Test)
                .HasKey(testanswers => testanswers.Id);

            builder.HasOne(testquestion => testquestion.TestQuestionAnswer)
                .WithMany(testanswers => testanswers.TestsAnswers)
                .HasForeignKey(testquestion => testquestion.AnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(test => test.Test)
                .WithMany(testanswer => testanswer.TestsAnswers)
                .HasForeignKey(test => test.TestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
