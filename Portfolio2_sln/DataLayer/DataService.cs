using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography.X509Certificates;

namespace DataLayer
{
    public class DataService : IDataService
    {
        private static ImdbContext _db = new ImdbContext();

        /* ------------
            TITLES
          ------------*/
        // can currently get with one query
        public IList<TitleBasics> GetTitles(string? titleType = null)
        {
            Console.WriteLine(titleType);
            var result = _db.TitleBasicss.ToList();
            if (titleType != null)
            {
                result = _db.TitleBasicss.Where(x => x.TitleType == (titleType)).ToList();
                Console.WriteLine(result.Count());
            }

            return result;
        }

        public TitleBasics GetTitle(string tconst)
        {
            var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == tconst);
            
            return temp;
        }

        public IList<BasicTitleModelDL> GetBasicTitles()
        {
            Console.WriteLine(_db.TitleBasicss.First().Tconst);
            var basicTitles = _db.TitleBasicss
                .Select(t => new BasicTitleModelDL 
                    { Tconst = t.Tconst,
                    TitleType = t.TitleType, 
                    PrimaryTitle = t.PrimaryTitle,
                    StartYear = t.StartYear
                }) 
                .ToList();

            return basicTitles;
        }

        public IList<ListTitleModelDL> GetListTitles()
        {


            var basicTitles = _db.TitleBasicss
                .OrderBy(t => t.Tconst)
            .Select(t => new ListTitleModelDL
            {
                    
                Tconst = t.Tconst,
                TitleType = t.TitleType,
                PrimaryTitle = t.PrimaryTitle,
                StartYear = t.StartYear,

                //Rating = GetRatingFromTitle(t.Tconst)
                Genres = GetGenresFromTitle(t.Tconst)

            })
            .ToList();
            
            //Console.WriteLine(basicTitles.Count());
            return null;
        }

        private static double? GetRatingFromTitle (string tconst)
        {
            var rating = new ImdbContext().TitleAvgRatings
                .Where(x => x.Tconst.Trim() == tconst.Trim())
                .FirstOrDefault(x => x.Tconst.Trim() == tconst.Trim());
            //.ToList();
            if (rating == null)
            {
                return null;
            }
            //Console.WriteLine(rating.AverageRating);
            var result_rating = rating.AverageRating;
            return result_rating;
        }



        public IList<DetailedTitleModelDL> GetDetailedTitles()
        {

            return null;
        }

        public BasicTitleModelDL GetBasicTitle(string tconst)
        {
            var basicTitle = _db.TitleBasicss
                .FirstOrDefault(x => x.Tconst == tconst);
            //.Where(x => x.Tconst == tconst)
            //.Include(x => x.TitleType)
            //.Include(t => t.Tconst)
            //.ToList();
            var basic = new BasicTitleModelDL
            {
                TitleType = basicTitle.TitleType,
                PrimaryTitle = basicTitle.PrimaryTitle,
                StartYear = basicTitle.StartYear,
                Tconst = tconst
            };

            return basic;
        }

        public ListTitleModelDL GetListTitle(string tconst)
        {
            return null;
        }

        public DetailedTitleModelDL GetDetailedTitle(string tconst)
        {

            return null;
        }


        public IList<TitleAka> GetTitleAkasByTitle(string tconst)
        {
            return _db.TitleAkas.Where(x => x.Tconst == tconst).ToList(); ;
        }

        


        private static  IList<string> GetGenresFromTitle(string tconst)
        {
            var genres =
                new ImdbContext().TitleGenres.Where(x => x.Tconst.Contains(tconst.Trim()))
            .Select(x => x.GenreName)
            .ToList();
            Console.WriteLine(genres.Count());
            return genres;
        }

        public IList<TitleBasics> GetTitlesByGenre(string genreName)
        {
            IList<TitleGenre> titleGenres =
                _db.TitleGenres.Where(x => x.GenreName.Contains(genreName)).ToList();


            var innerJoin = titleGenres.Join(
                    _db.TitleBasicss,
                    genre => genre.Tconst,
                    title => title.Tconst,
                    (genre, title) => new TitleBasics
                    {
                        Tconst        = title.Tconst,
                        TitleType     = title.TitleType,
                        PrimaryTitle  = title.PrimaryTitle,
                        OriginalTitle = title.OriginalTitle,
                        IsAdult       = title.IsAdult,
                        StartYear     = title.StartYear,
                        EndYear       = title.EndYear,
                        RunTimeMinutes = title.RunTimeMinutes
                        //TitleGenres = new List<TitleGenre>() { genre }
                    }
                    )
                    .ToList();


            return innerJoin;
            //return null;
        }





        public IList<TitleBasics> GetEpisodesFromTitle(string parentTconst)
        {
            Console.WriteLine(parentTconst);
            var episodes = _db.TitleEpisodes
                .Where(e => e.ParentTconst == parentTconst.Trim())
                //.Where(e => e.ParentTconst == parentTconst)
                .ToList();
            foreach (var episode in episodes)
            {
                Console.WriteLine(episode.Tconst);
            }
            Console.WriteLine(episodes.Count());

            var innerJoin = episodes.Join(
                    _db.TitleBasicss,
                    episode => episode.Tconst,
                    title => title.Tconst,
                    (episode, title) =>
                    title
                   
                    )
                    .ToList();
            Console.WriteLine(episodes.Count());
            Console.WriteLine(innerJoin.Count());

            return innerJoin;
        }



        public NameBasics GetName(string nconst) 
        {
            var temp = _db.NameBasicss.FirstOrDefault(x => x.Nconst == nconst);
            return temp;
        }

        public IList<NameBasics> GetNames() 
        {
            return _db.NameBasicss.ToList();
        }

        public IList<TitlePrincipal> GetTitlesPrincipalFromName(string nconst) 
        {
            IList<TitlePrincipal> titlePrincipals =
                _db.TitlePrincipals.Where(x => x.Nconst == nconst).ToList();

            //IList<TitleBasics> titleBasics =
            //    _db.TitleBasicss.Where(x => x.Tconst == titlePrincipals.Tconst).ToList();

            var innerJoin = titlePrincipals.Join(
                _db.NameBasicss,
                principal => principal.Nconst,
                name => name.Nconst,
                (principal, name) => new TitlePrincipal
                {
                    Tconst = principal.Tconst,
                    Nconst = principal.Nconst,
                    Category = principal.Category,
                }
                ).ToList();

            return innerJoin;
        }
        public OmdbData GetOmdbData(string tconst)
        {
            var temp = _db.omdbDatas.FirstOrDefault(x => x.Tconst == tconst);
            return temp;
        }

        public string GetPlot(string tconst)
        {
            var temp = GetOmdbData(tconst).Plot;
            return temp;
        }


















        /* ----------------------------------------------------------------------------------------------------------
                        USER 
        ---------------------------------------------------------------------------------------------------------- */

        public User GetUser(string username)
        {
            return _db.Users.FirstOrDefault(x => x.Username == username);
        }

        public IList<User> GetUsers()
        {
            return _db.Users.ToList();
        }

        public void CreateUser(string username, string password, string birthYear, string email)
        {
            User newUser = new User()
            {
                Username = username,
                Password = password,
                BirthYear = birthYear,
                Email = email
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

        }

        public bool DeleteUser(string username)
        {

            var product = GetUser(username);
            if (product == null)
            {
                return false;
            }
            _db.Users.Remove(GetUser(username));
            _db.SaveChanges();
            return true;
        }

        /*
         
         BOOKMARK TITLES
         
         */

        public BookmarkTitle GetBookmarkTitle(string username, string tconst)
        {
            return _db.BookmarkTitles.FirstOrDefault(x => x.Username == username && x.Tconst.Trim() == tconst.Trim());
        }

        public IList<BookmarkTitle> GetBookmarkTitles()
        {
            return _db.BookmarkTitles.ToList();
        }

        public IList<BookmarkTitle> GetBookmarkTitlesByUser(string username)
        {
            return _db.BookmarkTitles
                .Where(x => x.Username == username)
                .ToList();
        }

        public BookmarkTitle CreateBookmarkTitle(string username, string tconst, string annotation)
        {
            BookmarkTitle newBookmark = new BookmarkTitle()
            {
                Username = username,
                Tconst = tconst,
                Annotation = annotation
            };
            Console.WriteLine(newBookmark.Username + " " + newBookmark.Tconst + " " + newBookmark.Annotation);
            _db.BookmarkTitles.Add(newBookmark);
            _db.SaveChanges();
            return newBookmark;

        }

        public bool DeleteBookmarkTitle(string username, string tconst)
        {

            var product = GetBookmarkTitle(username, tconst);
            if (product == null)
            {
                return false;
            }
            _db.BookmarkTitles.Remove(GetBookmarkTitle(username, tconst));
            _db.SaveChanges();
            return true;
        }



























    }
}