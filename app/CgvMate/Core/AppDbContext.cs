using Microsoft.EntityFrameworkCore;

namespace CgvMate.Core
{
    public class AppDbContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<Theater> Theaters{ get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<OpenNotificationInfo> OpenNotificationInfos { get; set; }

        public AppDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Constants.APP_DB_PATH}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
                .HasKey(x => x.AreaCode);

            modelBuilder.Entity<Theater>()
                .HasKey(x=>x.TheaterCode);

            modelBuilder.Entity<Movie>(x =>
            {
                x.HasKey(y => y.Index);
                x.Ignore(a => a.ScreenTypes);
            });
            modelBuilder.Entity<OpenNotificationInfo>(x =>
            {
                x.HasKey(a => a.Id);
                x.Property(a => a.Id)
                 .ValueGeneratedOnAdd();

                x.HasOne(a => a.Movie)
                 .WithMany()
                 .HasForeignKey(a => a.MovieIndex);

                x.HasOne(a => a.Theater)
                 .WithMany()
                 .HasForeignKey(a => a.TheaterCode);
            });
        }
    }
}
