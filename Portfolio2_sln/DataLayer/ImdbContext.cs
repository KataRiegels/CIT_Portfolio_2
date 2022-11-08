
using Microsoft.EntityFrameworkCore;
using DataLayer.Models.TitleModels;
using DataLayer.Models.NameModels;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class ImdbContext : DbContext
    {
        const string ConnectionString = "host=localhost;db=imdb_backup;uid=postgres;pwd=Jse33pjp";
        //const string ConnectionString = "host=localhost;db=imdb;uid=postgres;pwd=password";
        public DbSet<TitleBasics> TitleBasicss { get; set; }
        public DbSet<NameBasics> NameBasicss { get; set; }




        // TITLES


        public DbSet<TitleGenre> TitleGenres { get; set; }
        public DbSet<TitleEpisode> TitleEpisodes { get; set; }
        public DbSet<TitleAka> TitleAkas { get; set; }


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
            genre_table.HasKey(t => new { t.Tconst, t.GenreName });
            genre_table.Property(x => x.Tconst).HasColumnName("tconst");
            genre_table.Property(x => x.GenreName).HasColumnName("genre");


            var name_table = modelBuilder.Entity<NameBasics>();
            name_table.ToTable("name_basics");
            name_table.HasKey(x => x.Nconst);
            name_table.Property(x => x.Nconst).HasColumnName("nconst");
            name_table.Property(x => x.PrimaryName).HasColumnName("primaryname");
            name_table.Property(x => x.BirthYear).HasColumnName("birthyear");
            name_table.Property(x => x.DeathYear).HasColumnName("deathyear");


            var episodeTable = modelBuilder.Entity<TitleEpisode>();
            episodeTable.ToTable("title_episode");
            episodeTable.HasKey(x => x.Tconst);
            episodeTable.Property(x => x.Tconst).HasColumnName("tconst");
            episodeTable.Property(x => x.ParentTconst).HasColumnName("parenttconst");
            episodeTable.Property(x => x.EpisodeNumber).HasColumnName("episodenumber");
            episodeTable.Property(x => x.SeasonNumber).HasColumnName("seasonnumber");













            // TITLES
            var titleAkaTable = modelBuilder.Entity<TitleAka>();
            titleAkaTable.ToTable("title_aka");
            titleAkaTable.HasKey(t => new { t.Tconst, t.Ordering });
            titleAkaTable.Property(x => x.Tconst).HasColumnName("tconst");
            titleAkaTable.Property(x => x.Ordering).HasColumnName("ordering");
            titleAkaTable.Property(x => x.Title).HasColumnName("title");
            titleAkaTable.Property(x => x.Region).HasColumnName("region");
            titleAkaTable.Property(x => x.IsOriginalTitle).HasColumnName("isoriginaltitle");




        }

    }
}
