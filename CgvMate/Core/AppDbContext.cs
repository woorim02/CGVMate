using Microsoft.EntityFrameworkCore;

namespace CgvMate.Core
{
	public class AppDbContext : DbContext
	{
		public DbSet<Area> Areas { get; set; }
		public DbSet<Theater> Theaters{ get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<TheaterGiveawayInfo> TheaterGiveawayInfos { get; set; }

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

			modelBuilder.Entity<Movie>()
				.HasKey(x => x.Index);
			modelBuilder.Entity<Movie>()
				.Ignore(x=>x.ScreenTypes);

			modelBuilder.Entity<TheaterGiveawayInfo>()
				.HasKey(x => new { x.TheaterCode, x.GiveawayIndex });

			modelBuilder.Entity<OpenNotificationInfo>()
				.HasKey(x => x.Id);
			modelBuilder.Entity<OpenNotificationInfo>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();
			modelBuilder.Entity<OpenNotificationInfo>()
				.HasOne(x => x.Movie);
			modelBuilder.Entity<OpenNotificationInfo>()
				.HasOne(x => x.TheaterGiveawayInfo);
		}
	}
}
