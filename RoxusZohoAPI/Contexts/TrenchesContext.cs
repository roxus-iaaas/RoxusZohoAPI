using Microsoft.EntityFrameworkCore;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Entities.TrenchesReportDB;

namespace RoxusZohoAPI.Contexts
{
    public class TrenchesContext : DbContext
    {

        public TrenchesContext(DbContextOptions<TrenchesContext> options)
            : base(options)
        {
        }

        public DbSet<SyncingRecord> SyncingRecords { get; set; }

        public DbSet<TrenchesPostcardReport> TrenchesPostcardReports { get; set; }

        public DbSet<ZohoTask> ZohoTasks { get; set; }

        public DbSet<LFLAddressData> LFLAddressDatas { get; set; }

        public DbSet<LFLNimbusRequest> LFLNimbusRequests { get; set; }

        public DbSet<LFLUPRNsFreeholdTitles> LFLUPRNsFreeholdTitles { get; set; }

        public DbSet<LFLUPRNsLeaseholdTitles> LFLUPRNsLeaseholdTitles { get; set; }

        public DbSet<ORPrimaryData> ORPrimaryDatas { get; set; }

        public DbSet<ORNimbusRequest> ORNimbusRequests { get; set; }

        public DbSet<ORUPRNsFreeholdTitles> ORUPRNsFreeholdTitles { get; set; }

        public DbSet<ORUPRNsLeaseholdTitles> ORUPRNsLeaseholdTitles { get; set; }

        public DbSet<OROwnerLinking> OROwnerLinkings { get; set; }

        public DbSet<ORTask> ORTasks { get; set; }

        public DbSet<CorporateOwnership> CorporateOwnerships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<LFLUPRNsFreeholdTitles>().HasKey(u => new
            {
                u.UPRN,
                u.FreeholdTitle
            });

            modelBuilder.Entity<LFLUPRNsLeaseholdTitles>().HasKey(u => new
            {
                u.UPRN,
                u.LeaseholdTitle
            });

            modelBuilder.Entity<ORUPRNsFreeholdTitles>().HasKey(u => new
            {
                u.UPRN,
                u.FreeholdTitle
            });

            modelBuilder.Entity<ORUPRNsLeaseholdTitles>().HasKey(u => new
            {
                u.UPRN,
                u.LeaseholdTitle
            });

            modelBuilder.Entity<OROwnerLinking>().HasKey(o => new
            {
                o.OwnerName,
                o.TitleNumber,
                o.OpenreachNumber
            });

        }
    }
}
