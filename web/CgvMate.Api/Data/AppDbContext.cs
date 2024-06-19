using CgvMate.Data.Entities.LotteCinema;
using CgvMate.Data.Entities.Cgv;
using Microsoft.EntityFrameworkCore;
using LotteEvent = CgvMate.Data.Entities.LotteCinema.Event;

namespace CgvMate.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<GiveawayEvent> CgvGiveawayEvents { get; set; }
    public DbSet<LotteEvent> LotteEvents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiveawayEvent>()
            .HasKey(x => x.EventIndex);
        modelBuilder.Entity<LotteEvent>()
            .HasKey(x => x.EventID);
    }
}
