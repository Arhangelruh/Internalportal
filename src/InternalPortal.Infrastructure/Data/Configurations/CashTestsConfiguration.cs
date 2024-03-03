using InternalPortal.Core.Models;
using InternalPortal.Infrastucture.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalPortal.Infrastructure.Data.Configurations
{
    /// <summary>
    /// EF Configuration for CashTests.
    /// </summary>
    public class CashTestsConfiguration : IEntityTypeConfiguration<CashTest>
    {
        public void Configure(EntityTypeBuilder<CashTest> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.CashTests, SchemaConstants.Test)
                .HasKey(test => test.Id);

            builder.Property(test => test.IsActual)
                .IsRequired();

            builder.Property(test => test.TestName)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.SqlMaxLengthShort);
        }
    }
}
