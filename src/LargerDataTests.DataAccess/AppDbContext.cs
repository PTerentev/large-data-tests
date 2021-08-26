using LargeDataTests.Domain.Entities;
using LargerDataTests.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LargerDataTests.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<TestDataWithJsonSerialization> DataWithJsonSerializations { get; set; }

        public DbSet<SystemJson_TestDataWithJsonSerialization> SystemJson_TestDataWithJsonSerializations { get; set; }

        public DbSet<SystemJsonCompressed_TestDataWithJsonSerialization> SystemJsonCompressed_TestDataWithJsonSerializations { get; set; }

        public DbSet<TestDataWithoutSerialization> DataWithoutSerializations { get; set; }

        public DbSet<ByteArray_SystemJson_TestDataWithJsonSerialization> ByteArray_SystemJson_TestDataWithJsonSerializations { get; set; }

        public DbSet<TestDataValues> TestDataValues { get; set; }

        public DbSet<CustomTestData> CustomTestDatas { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<TestDataWithJsonSerialization>()
                .Property(e => e.TestDataValues)
                .HasNewtonsoftJsonConversion();

            modelBuilder
                .Entity<SystemJson_TestDataWithJsonSerialization>()
                .Property(e => e.TestDataValues)
                .HasSystemJsonConversion();

            modelBuilder
                .Entity<ByteArray_SystemJson_TestDataWithJsonSerialization>()
                .Property(e => e.TestDataValues)
                .HasSystemJsonByteArrayConversion();

            modelBuilder
                .Entity<SystemJsonCompressed_TestDataWithJsonSerialization>()
                .Property(e => e.TestDataValues)
                .HasSystemJsonCompressedConversion();
        }
    }
}
