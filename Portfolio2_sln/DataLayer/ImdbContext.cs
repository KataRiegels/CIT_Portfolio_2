
using Microsoft.EntityFrameworkCore;
using DataLayer.Models.TitleModels;

namespace DataLayer
{
    public class ImdbContext : DbContext
    {
        const string ConnectionString = "host=localhost;db=imdb_backup;uid=postgres;pwd=Jse33pjp";
        public DbSet<TitleBasics> TitleBasicss { get; set; }
        public DbSet<TitleGenre> TitleGenres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var tb_mb = modelBuilder.Entity<TitleBasics>();

            tb_mb.ToTable("title_basics");
            tb_mb.HasKey(x => x.Tconst);
            tb_mb.Property(x => x.Tconst).HasColumnName("tconst");
            tb_mb.Property(x => x.TitleType).HasColumnName("titletype");
            tb_mb.Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
            tb_mb.Property(x => x.OriginalTitle).HasColumnName("originaltitle");
            tb_mb.Property(x => x.IsAdult).HasColumnName("isadult");
            tb_mb.Property(x => x.StartYear).HasColumnName("startyear");
            tb_mb.Property(x => x.EndYear).HasColumnName("endyear");
            tb_mb.Property(x => x.RunTimeMinutes).HasColumnName("runtimeminutes");

            var genre_table = modelBuilder.Entity<TitleGenre>();
            genre_table.ToTable("genre");
            genre_table.HasMany(b => b.TitleBasics).WithMany(g => g.TitleGenres);
            genre_table.HasKey(t => new { t.Tconst, t.GenreName });
            //genre_table.
            genre_table.Property(x => x.Tconst).HasColumnName("tconst");
            genre_table.Property(x => x.GenreName).HasColumnName("genre");

        }


    }
}
