using AgrootTerrarium.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AgrootTerrarium.Api.Data
{
    public class AgrootDbContext : DbContext
    {
        public AgrootDbContext(DbContextOptions<AgrootDbContext> options)
            :base (options){}
       
       public DbSet<Zone> Zones {get; set;}
       public DbSet<ScheduleEntry> ScheduleEntries {get; set;}

       public DbSet<MistEvent> MistEvents {get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Zone>()
                .HasMany(z => z.DailyTime)
                .WithOne(s => s.ParentZone)
                .HasForeignKey(s => s.ZoneId);


            modelBuilder.Entity<Zone>()
                .HasMany(z => z.HistoryLog)
                .WithOne(m => m.ParentZone)
                .HasForeignKey(m => m.ZoneId);    
        }

   }
}   