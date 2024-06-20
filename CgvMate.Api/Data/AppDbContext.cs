using CgvMate.Data.Entities.Cgv;
using CgvMate.Data.Entities.Megabox;
using Microsoft.EntityFrameworkCore;
using CgvGiveawayEvent = CgvMate.Data.Entities.Cgv.GiveawayEvent;
using LotteEvent = CgvMate.Data.Entities.LotteCinema.Event;
using MegaboxGiveawayEvent = CgvMate.Data.Entities.Megabox.GiveawayEvent;

namespace CgvMate.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<CgvGiveawayEvent> CgvGiveawayEvents { get; set; }
    public DbSet<LotteEvent> LotteEvents { get; set; }
    public DbSet<MegaboxGiveawayEvent> MegaboxGiveawayEvents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CgvGiveawayEvent>()
            .HasKey(x => x.EventIndex);
        modelBuilder.Entity<LotteEvent>()
            .HasKey(x => x.EventID);
        modelBuilder.Entity<MegaboxGiveawayEvent>()
            .HasKey(x => x.ID);
    }
}
