using InternalPortal.Core.Models;
using InternalPortal.Infrastructure.Data.Configurations;
using InternalPortal.Infrastucture.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Infrastucture.Data.Context
{
    public class InternalPortalContext : DbContext
    {
        public InternalPortalContext(DbContextOptions<InternalPortalContext> options)
            : base(options) { }

        /// <summary>
        /// Profiles.
        /// </summary>
        public DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// Tests.
        /// </summary>
        public DbSet<Test> Tests { get; set; }

        /// <summary>
        /// Test Topics.
        /// </summary>
        public DbSet<TestTopics> TestTopics { get; set; }

        /// <summary>
        /// Test questions.
        /// </summary>
        public DbSet<TestQuestions> TestQuestions { get; set; }

        /// <summary>
        /// Test Answers.
        /// </summary>
        public DbSet<TestQuestionAnswers> TestQuestionAnswers { get; set; }

        /// <summary>
        /// TestAnswers connection table.
        /// </summary>
        public DbSet<TestsAnswers> TestsAnswers { get; set; }

        /// <summary>
        /// Test score.
        /// </summary>
        public DbSet<TestScore> TestsScore { get; set; }

        /// <summary>
        /// Cash tests.
        /// </summary>
        public DbSet<CashTest> CashTests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new TestTopicsConfiguration());
            modelBuilder.ApplyConfiguration(new TestQuestionsConfiguration());
            modelBuilder.ApplyConfiguration(new TestQuestionAnswersConfiguration());
            modelBuilder.ApplyConfiguration(new TestTopicsConfiguration());
            modelBuilder.ApplyConfiguration(new TestsAnswersConfiguration());
            modelBuilder.ApplyConfiguration(new TestConfiguration());
            modelBuilder.ApplyConfiguration(new TestsScoreConfiguration());
            modelBuilder.ApplyConfiguration(new CashTestsConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
