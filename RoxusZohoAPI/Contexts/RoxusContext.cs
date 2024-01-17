using Microsoft.EntityFrameworkCore;
using RoxusZohoAPI.Entities.RoxusDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Contexts
{
    public class RoxusContext : DbContext
    {
        public RoxusContext(DbContextOptions<RoxusContext> options)
            : base(options)
        {
        }

        public DbSet<AppConfiguration> AppConfigurations { get; set; }

        public DbSet<ApiLogging> ApiLoggings { get; set; }

        public DbSet<TrenchesTask> TrenchesTasks { get; set; }

        public DbSet<HinetsRecord> HinetsRecords { get; set; }

        public DbSet<TWPowerBIRecord> TWPowerBIRecords { get; set; }

        public DbSet<ApiTransactionQueue> ApiTransactionQueues { get; set; }

        public DbSet<DocmailRecord> DocmailRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrenchesTask>().HasKey(t => new { t.TaskId, t.ProjectId });

            modelBuilder.Entity<ApiLogging>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
