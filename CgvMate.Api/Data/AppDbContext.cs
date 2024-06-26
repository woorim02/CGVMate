﻿using CgvMate.Data.Entities.Cgv;
using CgvMate.Data.Entities.LotteCinema;
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
    public DbSet<GiveawayEventKeyword> LotteGiveawayEventKeywords { get; set; }
    public DbSet<LotteGiveawayEventModel> LotteGiveawayEventModels { get; set; }
    public DbSet<MegaboxGiveawayEvent> MegaboxGiveawayEvents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CgvGiveawayEvent>()
            .HasKey(x => x.EventIndex);
        modelBuilder.Entity<LotteEvent>(entity =>
        {
            entity.HasKey(x => x.EventID);
            entity.Ignore(x => x.ImageAlt);
            entity.Ignore(x=>x.EventNtc);
        });
        modelBuilder.Entity<MegaboxGiveawayEvent>()
            .HasKey(x => x.ID);

        modelBuilder.Entity<GiveawayEventKeyword>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id)
                  .ValueGeneratedOnAdd();
            entity.HasIndex(x => x.Keyword)
                  .IsUnique();
        });

        modelBuilder.Entity<LotteGiveawayEventModel>(entity =>
        {
            entity.HasKey(x => x.EventID);
        });
    }
}
