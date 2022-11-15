using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataLayer
{
    public class DataService : IDataService
    {
        private static ImdbContext _db = new ImdbContext();

        /* ------------
            TITLES
          ------------*/
        // can currently get with one query
        public IList<TitleBasics> GetTitles(int page = 0, int pageSize = 20)
        {
            var result = _db.TitleBasicss.Skip(page * pageSize).Take(pageSize).ToList();
            //Console.WriteLine("-------------------------------------");
            //if (titleType != null)
            //{
                result = _db.TitleBasicss.Skip(page * pageSize).Take(pageSize).ToList();
                //Console.WriteLine(result.Count());
            //}

            return result;
        }

        public TitleBasics GetTitle(string tconst)
        {
            var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == tconst);

            return temp;
        }

        public BasicTitleModelDL GetBasicTitle(string tconst)
        {
            var basicTitle = _db.TitleBasicss
                .FirstOrDefault(x => x.Tconst.Trim() == tconst.Trim());
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

        public IList<BasicTitleModelDL> GetBasicTitles(int page = 0, int pageSize = 20)
        {
            Console.WriteLine(_db.TitleBasicss.First().Tconst);
            var basicTitles = _db.TitleBasicss
                .Select(t => new BasicTitleModelDL
                {
                    Tconst = t.Tconst,
                    TitleType = t.TitleType,
                    PrimaryTitle = t.PrimaryTitle,
                    StartYear = t.StartYear
                })
                .Skip(page * pageSize).Take(pageSize).ToList();

            return basicTitles;
        }

        public IList<ListTitleModelDL> GetListTitles(int page, int pageSize)
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

            //    }).Skip(page * pageSize).Take(pageSize)..ToList();
            //return titles;
            return null;
        }




        public IList<DetailedTitleModelDL>? GetDetailedTitles(int page, int pageSize)
        {



            var titles = _db.FullViewTitles

                .ToList()
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
                    genre = model.Select(m => m.genre).Distinct().Skip(page * pageSize).Take(pageSize).ToList()
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();



            var temp = new List<DetailedTitleModelDL>();
            //return titles;
            return titles;
        }




        public ListTitleModelDL GetListTitle(string tconst)
        {
            return null;
        }

        public DetailedTitleModelDL GetDetailedTitle(string tconst)
        {



            return null;
        }




        public NameBasics GetName(string nconst)
        {
            var temp = _db.NameBasicss.FirstOrDefault(x => x.Nconst == nconst);
            return temp;
        }

        public IList<NameBasics> GetNames(int page = 0, int pageSize = 20)
        {
            return _db.NameBasicss.Skip(page * pageSize).Take(pageSize).ToList();
        }




        //----------------------------------------------------------------------------------------------
        //             NAME
        //----------------------------------------------------------------------------------------------


        public IList<BasicNameModelDL> GetBasicNames(int page = 0, int pageSize = 20)
        {
            var basicnames = _db.NameBasicss
                .Select(n => new BasicNameModelDL
                {
                    Nconst = n.Nconst,
                    PrimaryName = n.PrimaryName
                })
                .Skip(page * pageSize).Take(pageSize).ToList();

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

      

        //public IList<ListNameModelDL> GetListNames(int page = 0, int pageSize = 20)
        //{
        //    var names = GetDetailedNames()
        //        .Select(x => new ListNameModelDL()
        //        {
        //            Nconst = x.Nconst,
        //            PrimaryName = x.PrimaryName,
        //            //Profession = x.Profession,
        //            KnownForTitle = x.KnownForTitle,
        //            StartYear = x.StartYear,
        //            TitleType = x.TitleType,
        //            Tconst = x.Tconst
        //        }).Skip(page * pageSize).Take(pageSize).ToList();

        //    return names;
        //}


        //----------------------------------------------------------------------------------------------
        //         NAME HELPERS
        //----------------------------------------------------------------------------------------------












              //----------------------------------------------------------------------------------------------
        //             NAME
        //----------------------------------------------------------------------------------------------


     


        public IList<ListNameModelDL> GetListNames(int page = 0, int pageSize = 20)
        {

            var query =
                _db.NameBasicss.ToList().GroupJoin(_db.NameKnownFors,
                       basics => basics.Nconst,
                       knownFor => knownFor.Nconst,
                       (basics, knownFors) =>
                       new
                       {
                           //basics,
                           Nconst = basics.Nconst,
                           PrimaryName = basics.PrimaryName,
                           //PrimaryName = basics.PrimaryName,
                           KnownForTitle = knownFors.Select(x => x.Tconst)
                       }
                           )
                        // .GroupJoin(_db.TitleBasicss,
                        //basics => basics.KnownForTitle,
                        //knownFor => knownFor.Tconst,
                        //(basics, knownFors) =>
                        //new
                        //{
                        //    Nconst = basics.basics.Nconst,
                        //    PrimaryName = basics.basics.PrimaryName,
                        //    KnownForTitle = knownFors.Select(x => x.Tconst)
                        //})
                        ;

            var names2 = query.ToList()
                .GroupBy(t => t.Nconst, (key, model) => new ListNameModelDL
                {
                    Nconst = key,
                    PrimaryName = model.First().PrimaryName,
                    KnownForTitleBasics = model.First().KnownForTitle.Any() ? GetBasicTitle(model.First().KnownForTitle.First()) : null
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();

            return names2;




            //    var names = _db.FullViewNames

            //        .ToList()
            //        //.GroupBy(t => t.Tconst,t => t.genre, (key, genre) => new DetailedTitleModelDL
            //        .GroupBy(t => t.Nconst, (key, model) => new ListNameModelDL
            //        {
            //            Nconst = key,
            //            PrimaryName = model.First().PrimaryName,
            //            KnownForTitleBasics = model.First().KnwonForTconst != null ? GetBasicTitle(model.First().KnwonForTconst) : null
            //            //KnownForTitle = model.First().KnwonForTconst,
            //            //TitleType = model.First().KnwonForTconst != null ? GetTitle(model.First().KnwonForTconst).TitleType : null
            //            //TitleType = model.First().KnwonForTconst != null ? model.First().KnwonForTconst : null
            //            //StartYear = model.First().KnwonForTconst,

            //            //DeathYear = model.First().DeathYear,
            //            //Professions = model.Select(p => p.Profession).Distinct().ToList(),
            //            //KnwonForTconst = model.Select(m => m.KnwonForTconst).Distinct().ToList(),
            //            //Characters = model.Select(m => new Tuple<string, string>(m.Character, m.CharacterTconst)).Distinct().ToList(),
            //            //Jobs = model.AsEnumerable().Select(m => new Tuple<string, string>(m.Job, m.JobTconst)).Distinct().ToList()
            //            //plot = model.First().plot,
            //            //poster = model.First().poster,
            //            ////Tconst = obj.Tconst,
            //            //genre = model.Select(m => m.genre).Distinct().ToList()
            //        }
            //        ).Take(21).ToList();
            //return names;



            //return null;
        }


        public IList<DetailedNameModelDL>? GetDetailedNames(int page = 0, int pageSize = 20)
        {
            var names = _db.FullViewNames

                .ToList()
                //.GroupBy(t => t.Tconst,t => t.genre, (key, genre) => new DetailedTitleModelDL
                .GroupBy(t => t.Nconst, (key, model) => new DetailedNameModelDL
                {
                    Nconst = key,
                    PrimaryName = model.First().PrimaryName,
                    BirthYear = model.First().BirthYear,
                    DeathYear = model.First().DeathYear,
                    Professions = model.Select(p => p.Profession).Distinct().ToList(),
                    KnwonForTconst = model.Select(m => m.KnwonForTconst).Distinct().ToList(),
                    Characters = model.Select(m => new Tuple<string, string>(m.Character, m.CharacterTconst)).Distinct().ToList(),
                    Jobs = model.AsEnumerable().Select(m => new Tuple<string, string>(m.Job, m.JobTconst)).Distinct().ToList()
                    //plot = model.First().plot,
                    //poster = model.First().poster,
                    ////Tconst = obj.Tconst,
                    //genre = model.Select(m => m.genre).Distinct().ToList()
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();
            return names;
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

 

























    }
}