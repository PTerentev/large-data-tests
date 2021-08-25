using LargeDataTests.Domain.Entities;
using LargerDataTests.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LargerDataTests.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<TestDataWithJsonSerialization> DataWithJsonSerializations { get; set; }

        public DbSet<TestDataWithoutSerialization> DataWithoutSerializations { get; set; }

        public DbSet<TestDataValues> TestDataValues { get; set; }

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
                .HasJsonConversion();
        }
    }
}
