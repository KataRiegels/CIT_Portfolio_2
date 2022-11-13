using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using System.Runtime.CompilerServices;
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

            var result = _db.TitleBasicss.ToList();
            Console.WriteLine("-------------------------------------");
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
            //var titles = GetDetailedTitles()
            //    .Select(x => new ListTitleModelDL()
            //    {
            //        Tconst = x.Tconst,
            //        PrimaryTitle = x.PrimaryTitle,
            //        StartYear = x.startyear,
            //        TitleType = x.titletype,
            //        runtime = x.runtime,
            //        Rating = x.rating,
            //        Genres = x.genre

            //    }).Take(20).ToList();
            //return titles;
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



        public IList<DetailedTitleModelDL>? GetDetailedTitles()

                    /*
            fullview.where(tconst = tconst).select(genre).add(list)


            */

            
        {

            //var titles = _db.FullViews
            //IList<DetailedTitleModelDL> titles = (IList<DetailedTitleModelDL>)_db.FullViews
            var titles = _db.FullViews
               
                .ToList()
                //.GroupBy(t => t.Tconst,t => t.genre, (key, genre) => new DetailedTitleModelDL
                .GroupBy(t => t.Tconst, (key, model) => new DetailedTitleModelDL
                {
                    PrimaryTitle = model.First().PrimaryTitle,
                    startyear = model.First().startyear,
                    titletype = model.First().titletype,
                    runtime = model.First().runtime,
                    rating = model.First().rating,
                    plot = model.First().plot,
                    poster = model.First().poster,
                    Tconst = key,
                    //Tconst = obj.Tconst,
                    genre =  model.Select(m => m.genre).ToList()
                }
                ).Take(20).ToList();
                
                //.AsEnumerable(
                    
                //)
                Console.WriteLine("sdfkslfk");
            foreach (var title in titles)
            {
                Console.WriteLine(title);
            
            //foreach (var item in title)
            //    {
            //    Console.WriteLine(item.genre);
                    
            //    }
            }

           var temp = new List<DetailedTitleModelDL>();
            //return titles;
            return titles;
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



        //----------------------------------------------------------------------------------------------
        //             NAME
        //----------------------------------------------------------------------------------------------


        public IList<BasicNameModelDL> GetBasicNames()
        {
            var basicnames = _db.NameBasicss
                .Select(n => new BasicNameModelDL
                {
                    Nconst = n.Nconst,
                    PrimaryName = n.PrimaryName
                })
                .ToList();

            return basicnames;
        }

        public BasicNameModelDL GetBasicName(string nconst)
        {
            var namebasic = _db.NameBasicss.FirstOrDefault(x => x.Nconst == nconst);

            var basicname = new BasicNameModelDL()
            {
                Nconst = nconst,
                PrimaryName = namebasic.PrimaryName
            };

            return basicname;
        }

        public IList<DetailedNameModelDL>? GetDetailedNames()
        {
            //var names = _db.DetailedNames.Select(n => n).ToList();
            //return names;
            return null;
        }

        public IList<ListNameModelDL> GetListNames()
        {
            var names = GetDetailedNames()
                .Select(x => new ListNameModelDL()
                {
                    Nconst = x.Nconst,
                    Primaryname = x.Primaryname,
                    Profession = x.Profession,
                    KnownForTitle = x.KnownForTitle,
                    StartYear = x.StartYear,
                    TitleType = x.TitleType,
                    Tconst = x.Tconst
                }).ToList();

            

            return names;
        }
        public IList<DetailedActorModel> GetDetailedActors()
        {
            return null;
        }

        public IList<DetailedProducerModel> GetDetailedProducers()
        {
            return null;
        }


        //----------------------------------------------------------------------------------------------
        //         NAME HELPERS
        //----------------------------------------------------------------------------------------------

        public string GetProfession(string nconst)
        {
            string temp = "";
            try { temp = _db.NameProfessions.FirstOrDefault(x => x.Nconst == nconst).Profession; } 
            catch
            {
                return "";
            }
            
            Console.WriteLine(temp);
            return temp;
        }

        public string GetKnownFor(string nconst)
        {
            string temp = "";
            try { temp = _db.NameKnownFors.FirstOrDefault(x => x.Nconst == nconst).Tconst; }
            catch
            {
                return "tt10382912";
            }
            Console.WriteLine(temp);
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

        public void CreateBookmarkTitle(string username, string tconst, string annotation)
        {
            BookmarkTitle newBookmark = new BookmarkTitle()
            {
                Username = username,
                Tconst = tconst,
                Annotation = annotation
            };

            _db.BookmarkTitles.Add(newBookmark);
            _db.SaveChanges();

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

        public IList<BookmarkTitleTest> GetTitleBookmarks()
        {
            Console.WriteLine("Call function from Entity Framework");
            using var ctx = new ImdbContext();

            string name = "user";

            var result = ctx.BookmarkTitlesTests.FromSqlInterpolated($"select * from select_title_bookmark({name})");




            //var bookmark = ctx.BookmarkTitles.Where(x => x.Username == name).ToList();

            Console.WriteLine(result.ToList());

            //ctx.SaveChanges();

            return result.ToList();

        }

























    }
}