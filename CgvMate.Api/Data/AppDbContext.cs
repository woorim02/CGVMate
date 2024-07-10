using CgvMate.Data.Entities.Cgv;
using CgvMate.Data.Entities.LotteCinema;
using CgvMate.Data.Entities.Megabox;
using Microsoft.EntityFrameworkCore;
using CgvGiveawayEvent = CgvMate.Data.Entities.Cgv.GiveawayEvent;
using CgvCuponEvent = CgvMate.Data.Entities.Cgv.CuponEvent;
using LotteEvent = CgvMate.Data.Entities.LotteCinema.Event;
using MegaboxGiveawayEvent = CgvMate.Data.Entities.Megabox.GiveawayEvent;
using MegaboxCuponEvent = CgvMate.Data.Entities.Megabox.CuponEvent;
using CgvMate.Api.Entities;

namespace CgvMate.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<CgvGiveawayEvent> CgvGiveawayEvents { get; set; }
    public DbSet<CgvCuponEvent> CgvCuponEvents { get; set; }
    public DbSet<LotteEvent> LotteEvents { get; set; }
    public DbSet<GiveawayEventKeyword> LotteGiveawayEventKeywords { get; set; }
    public DbSet<LotteGiveawayEventModel> LotteGiveawayEventModels { get; set; }
    public DbSet<MegaboxGiveawayEvent> MegaboxGiveawayEvents { get; set; }
    public DbSet<MegaboxCuponEvent> MegaboxCuponEvents { get; set; }

    public DbSet<Board> Boards { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CgvGiveawayEvent>()
            .HasKey(x => x.EventIndex);
        modelBuilder.Entity<CgvCuponEvent>()
            .HasKey(x => x.EventId);
        modelBuilder.Entity<LotteEvent>(entity =>
        {
            entity.HasKey(x => x.EventID);
            entity.Ignore(x => x.ImageAlt);
            entity.Ignore(x=>x.EventNtc);
        });
        modelBuilder.Entity<MegaboxGiveawayEvent>()
            .HasKey(x => x.ID);

        modelBuilder.Entity<MegaboxCuponEvent>()
            .HasKey(x => x.EventNo);

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


        // Board entity configuration
        modelBuilder.Entity<Board>(builder =>
        {
            builder.HasKey(b => b.Id);
            builder.HasMany(b => b.Posts)
                   .WithOne(p => p.Board)
                   .HasForeignKey(p => p.BoardId);
        });

        // Post entity configuration
        modelBuilder.Entity<Post>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(p => p.UserId);
            builder.HasMany(p => p.Comments)
                   .WithOne(c => c.Post)
                   .HasForeignKey(c => c.PostId);
        });

        // Comment entity configuration
        modelBuilder.Entity<Comment>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.Post)
                   .WithMany(p => p.Comments)
                   .HasForeignKey(c => c.PostId);
            builder.HasOne(c => c.ParentComment)
                   .WithMany(c => c.Children)
                   .HasForeignKey(c => c.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId);
        });

        // User entity configuration
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Ignore(u => u.PostCount);
            builder.Ignore(u => u.CommentCount);
            builder.HasMany(u => u.Posts)
                   .WithOne(p => p.User)
                   .HasForeignKey(p => p.UserId);
            builder.HasMany(u => u.Comments)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId);
        });
    }
}
