
using Microsoft.EntityFrameworkCore;
using DataLayer.Models.TitleModels;
using DataLayer.Models.NameModels;
using DataLayer.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DomainModels.NameModels;

namespace DataLayer
{
    public class ImdbContext : DbContext
    {
        const string ConnectionString = "host=localhost;db=imdb_backup;uid=postgres;pwd=Jse33pjp";
        //const string ConnectionString = "host=localhost;db=imdb;uid=postgres;pwd=password";
        public DbSet<TitleBasics> TitleBasicss { get; set; }
        //public DbSet<DetailedTitleDTO> DetailedTitles { get; set; }
        public DbSet<NameBasics> NameBasicss { get; set; }
        //public DbSet<DetailedNameDTO> DetailedNames { get; set; }
        public DbSet<FullTitleViewModel> FullViewTitles { get; set; }
        public DbSet<FullNameViewModel> FullViewNames { get; set; }

        public DbSet<NameProfession> NameProfessions { get; set; }
        public DbSet<NameKnownFor> NameKnownFors { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<BookmarkTitle> BookmarkTitles { get; set; }
        public DbSet<BookmarkName> BookmarkNames { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }

        public DbSet<UserSearch> UserSearches { get; set; }

        public DbSet<TitleSearchResult> SearchTitleResults{ get; set; }
        public DbSet<NameSearchResult> SearchNameResults{ get; set; }

        public DbSet<BookmarkTitleTest> BookmarkTitlesTests { get; set; }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Job> Jobs { get; set; }

        // TITLES

        public DbSet<TitleAvgRating> TitleAvgRatings { get; set; }
        
        public DbSet<TitleGenre> TitleGenres { get; set; }
        public DbSet<TitleEpisode> TitleEpisodes { get; set; }
        public DbSet<TitlePrincipal> TitlePrincipals { get; set; }
        public DbSet<OmdbData> omdbDatas { get; set; }
        public DbSet<TitleAka> TitleAkas { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookmarkTitleTest>().HasNoKey();
            modelBuilder.Entity<BookmarkTitleTest>().Property(x => x.PrimaryName).HasColumnName("name");
            modelBuilder.Entity<BookmarkTitleTest>().Property(x => x.Tconst).HasColumnName("tconst");
            modelBuilder.Entity<BookmarkTitleTest>().Property(x => x.Annotation).HasColumnName("annotation");


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

            var KnownFor_table = modelBuilder.Entity<NameKnownFor>();
            KnownFor_table.ToTable("known_for");
            KnownFor_table.HasKey(x => new { x.Tconst, x.Nconst});
            KnownFor_table.Property(x => x.Tconst).HasColumnName("tconst");
            KnownFor_table.Property(x => x.Nconst).HasColumnName("nconst");

            var name_profession = modelBuilder.Entity<NameProfession>();
            name_profession.ToTable("profession");
            name_profession.HasKey(x => x.Nconst);
            name_profession.Property(x => x.Nconst).HasColumnName("nconst");
            name_profession.Property(x => x.Profession).HasColumnName("profession");


            var characterTable = modelBuilder.Entity<Character>();
            characterTable.ToTable("character");
            characterTable.HasKey(x => new { x.Tconst, x.Nconst, x.CharacterName});
            characterTable.Property(x => x.Nconst).HasColumnName("nconst");
            characterTable.Property(x => x.Tconst).HasColumnName("tconst");
            characterTable.Property(x => x.CharacterName).HasColumnName("character");

            var jobTable = modelBuilder.Entity<Job>();
            jobTable.ToTable("job");
            jobTable.HasKey(x => new { x.Tconst, x.Nconst, x.JobName });
            jobTable.Property(x => x.Nconst).HasColumnName("nconst");
            jobTable.Property(x => x.Tconst).HasColumnName("tconst");
            jobTable.Property(x => x.JobName).HasColumnName("job");



            var list_names = modelBuilder.Entity<FullNameViewModel>();
            list_names.ToView("detailed_names");
            list_names.HasNoKey();
            list_names.Property(x => x.Nconst).HasColumnName("nconst");
            list_names.Property(x => x.Character).HasColumnName("character");
            list_names.Property(x => x.PrimaryName).HasColumnName("primaryname");
            list_names.Property(x => x.BirthYear).HasColumnName("birthyear");
            list_names.Property(x => x.DeathYear).HasColumnName("deathyear");
            list_names.Property(x => x.Profession).HasColumnName("profession");
            list_names.Property(x => x.KnwonForTconst).HasColumnName("kf_tconst");
            list_names.Property(x => x.Character).HasColumnName("character");
            list_names.Property(x => x.CharacterTconst).HasColumnName("ch_tconst");
            list_names.Property(x => x.Job).HasColumnName("job");
            list_names.Property(x => x.JobTconst).HasColumnName("job_tconst");


            // Usser

            var userTable = modelBuilder.Entity<User>();
            userTable.ToTable("users");
            userTable.HasKey(x => x.Username);
            userTable.Property(x => x.Username).HasColumnName("username");
            userTable.Property(x => x.Password).HasColumnName("password");
            userTable.Property(x => x.BirthYear).HasColumnName("birthyear");
            userTable.Property(x => x.Email).HasColumnName("email");
            //userTable.Property(x => x.).HasColumnName("");

            var userBookmarkNameTable = modelBuilder.Entity<BookmarkName>();
            userBookmarkNameTable.ToTable("bookmark_name");
            userBookmarkNameTable.HasKey(x =>  new { x.Username, x.Nconst });
            userBookmarkNameTable.Property(x => x.Username).HasColumnName("username");
            userBookmarkNameTable.Property(x => x.Nconst).HasColumnName("nconst");
            userBookmarkNameTable.Property(x => x.Annotation).HasColumnName("annotation");
            //.Property(x => x.BirthYear).HasColumnName("birthyear");
            //.Property(x => x.Email).HasColumnName("email");

            var userBookmarkTitleTable = modelBuilder.Entity<BookmarkTitle>();
            userBookmarkTitleTable.ToTable("bookmark_title");
            userBookmarkTitleTable.HasKey(x => new { x.Username, x.Tconst });
            userBookmarkTitleTable.Property(x => x.Username).HasColumnName("username");
            userBookmarkTitleTable.Property(x => x.Tconst).HasColumnName("tconst");
            userBookmarkTitleTable.Property(x => x.Annotation).HasColumnName("annotation");

            var userRatingTable = modelBuilder.Entity<UserRating>();
            userRatingTable.ToTable("user_rating");
            userRatingTable.HasKey(x => new { x.Username, x.Tconst });
            userRatingTable.Property(x => x.Username).HasColumnName("username");
            userRatingTable.Property(x => x.Tconst).HasColumnName("tconst");
            userRatingTable.Property(x => x.Rating).HasColumnName("rating");
            userRatingTable.Property(x => x.Date).HasColumnName("date");


            var userSearchTable = modelBuilder.Entity<UserSearch>();
            userSearchTable.ToTable("user_search");
            userSearchTable.HasKey(x => new { x.Username, x.SearchId });
            userSearchTable.Property(x => x.Username).HasColumnName("username");
            userSearchTable.Property(x => x.SearchId).HasColumnName("search_id");
            userSearchTable.Property(x => x.Date).HasColumnName("date");
            userSearchTable.Property(x => x.SearchContent).HasColumnName("search_content");
            userSearchTable.Property(x => x.SearchCategory).HasColumnName("search_category");

            var searchResultTitles = modelBuilder.Entity<TitleSearchResult>();
            searchResultTitles.HasNoKey();
            searchResultTitles.Property(x => x.Tconst).HasColumnName("tconst");
            searchResultTitles.Property(x => x.PrimaryTitle).HasColumnName("primarytitle");

            var searchResultNames = modelBuilder.Entity<NameSearchResult>();
            searchResultNames.HasNoKey();
            searchResultNames.Property(x => x.Nconst).HasColumnName("nconst");
            searchResultNames.Property(x => x.PrimaryName).HasColumnName("primaryname");

            // TITLES
            var titleAkaTable = modelBuilder.Entity<TitleAka>();
            titleAkaTable.ToTable("title_aka");
            titleAkaTable.HasKey(t => new { t.Tconst, t.Ordering });
            titleAkaTable.Property(x => x.Tconst).HasColumnName("tconst");
            titleAkaTable.Property(x => x.Ordering).HasColumnName("ordering");
            titleAkaTable.Property(x => x.Title).HasColumnName("title");
            titleAkaTable.Property(x => x.Region).HasColumnName("region");
            titleAkaTable.Property(x => x.IsOriginalTitle).HasColumnName("isoriginaltitle");


            var titlePrincipleTable = modelBuilder.Entity<TitlePrincipal>();
            titlePrincipleTable.ToTable("title_principal");
            titlePrincipleTable.HasKey(t => new { t.Tconst, t.Nconst, t.Category });
            titlePrincipleTable.Property(x => x.Tconst).HasColumnName("tconst");
            titlePrincipleTable.Property(x => x.Nconst).HasColumnName("nconst");
            titlePrincipleTable.Property(x => x.Category).HasColumnName("category");

            var titleRatingsTable = modelBuilder.Entity<TitleAvgRating>();
            titleRatingsTable.ToTable("title_rating");
            titleRatingsTable.HasKey(x => x.Tconst);
            titleRatingsTable.Property(x => x.Tconst).HasColumnName("tconst");
            titleRatingsTable.Property(x => x.AverageRating).HasColumnName("averagerating");
            titleRatingsTable.Property(x => x.NumVotes).HasColumnName("numvotes");


            var fullTitleView = modelBuilder.Entity<FullTitleViewModel>();
            fullTitleView.ToView("detailed_titles");
            fullTitleView.HasNoKey();
            fullTitleView.Property(x => x.Tconst).HasColumnName("tconst");
            fullTitleView.Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
            fullTitleView.Property(x => x.StartYear).HasColumnName("startyear");
            fullTitleView.Property(x => x.TitleType).HasColumnName("titletype");
            fullTitleView.Property(x => x.Runtime).HasColumnName("runtimeminutes");
            fullTitleView.Property(x => x.Rating).HasColumnName("averagerating");
            fullTitleView.Property(x => x.Genre).HasColumnName("genre");
            fullTitleView.Property(x => x.Plot).HasColumnName("plot");
            fullTitleView.Property(x => x.Poster).HasColumnName("poster");
            fullTitleView.Property(x => x.ParentTconst).HasColumnName("parenttconst");
            fullTitleView.Property(x => x.SeasonNumber).HasColumnName("seasonnumber");
            fullTitleView.Property(x => x.EpisodeNumber).HasColumnName("episodenumber");

            //fullTitleView.Property(x => x.relatedName).HasColumnName("primaryname");

            fullTitleView.Property(x => x.RelatedName).HasColumnName("plot");



            //titlePrincipleTable.Property(x => x.).HasColumnName("");

            var omdbTalbe = modelBuilder.Entity<OmdbData>();
            omdbTalbe.ToTable("omdb_data");
            omdbTalbe.HasKey(x => x.Tconst);
            omdbTalbe.Property(x => x.Tconst).HasColumnName("tconst");
            omdbTalbe.Property(x => x.Poster).HasColumnName("poster");
            omdbTalbe.Property(x => x.Plot).HasColumnName("plot");
        }

    }
}
