using DataLayer.DataTransferObjects;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataLayer.DataServices
{
    public class DataServiceUser : IDataServiceUser
    {
        private static ImdbContext _db = new ImdbContext();



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

        public BookmarkName GetBookmarkName(string username, string nconst)
        {
            return _db.BookmarkNames.FirstOrDefault(x => x.Username == username && x.Nconst.Trim() == nconst.Trim());
        }


        public IList<BookmarkTitle> GetBookmarkTitles()
        {
            return _db.BookmarkTitles.ToList();
        }


        public bool CreateBookmarkName(string username, string nconst, string annotation)
        {
            using var db = new ImdbContext();

            var newBootkmark = new BookmarkName()
            {
                Username = username,
                Annotation = annotation,
                Nconst = nconst
            };

            db.BookmarkNames.Add(newBootkmark);
            db.SaveChanges();

            return true;
        }

        public IList<ListNameModelDL> GetBookmarkNamesByUser(string username)
        {
            using var db = new ImdbContext();

            var bookmarksFilter = db.BookmarkNames
                .Where(x => x.Username == username)
                .ToList();

            //var result = GetFilteredTitles(bookmarksFilter);
            var result = new DataServiceNames()
                .GetFilteredNames(bookmarksFilter
                    .Select(x => new NconstObject { Nconst = x.Nconst }).ToList());

            return result;
        }

        public IList<ListTitleModelDL> GetBookmarkTitlesByUser(string username)
        {
            using var db = new ImdbContext();

            var bookmarksFilter = db.BookmarkTitles
                .Where(x => x.Username == username)
                .ToList();

            //var result = GetFilteredTitles(bookmarksFilter);
            var result = new DataServiceTitles()
                .GetFilteredTitles(bookmarksFilter
                    .Select(x => new TconstObject { Tconst = x.Tconst }).ToList());

            return result;
        }

        public IList<ListTitleModelDL> GetFilteredTitles(List<BookmarkTitle> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();

            // Filters the FullViewTitles to only have those returned from the string search
            var filteredTitles = db.FullViewTitles.ToList()
                .Join(searchedTitles,
                    fullView => fullView.Tconst,
                    searchResults => searchResults.Tconst,
                    (fullView, searchResults)
                                  => fullView
                    );

            // Groups the titles so we can make a list of genres for each title
            // and creates the list form DTO
            var groupedTitles = filteredTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new ListTitleModelDL
                {
                    BasicTitle = new BasicTitleModelDL
                    {
                        Tconst = model.First().Tconst,
                        PrimaryTitle = model.First().PrimaryTitle,
                        StartYear = model.First().StartYear,
                        TitleType = model.First().TitleType,
                    },
                    Runtime = model.First().Runtime,
                    Rating = model.First().Rating,
                    Genres = model.Select(m => m.Genre).Distinct().ToList(),
                    ParentTitle = string.IsNullOrEmpty(model.FirstOrDefault().ParentTconst)
                                    ? null
                                    : new DataService().GetBasicTitle(model.FirstOrDefault().ParentTconst)
                })
                .Skip(page * pageSize).Take(pageSize)
                .ToList();


            return groupedTitles;
        }






        public int CreateBookmarkTitle(string username, string tconst, string annotation)
        {
            var newBookmark = new BookmarkTitle()
            {
                Username = username,
                Tconst = tconst,
                Annotation = annotation
            };

            _db.BookmarkTitles.Add(newBookmark);
            var result = _db.SaveChanges();

            return result;

        }

        public bool DeleteBookmarkTitle(string username, string tconst)
        {
            using var db = new ImdbContext();

            var product = GetBookmarkTitle(username, tconst);
            if (product == null)
            {
                return false;
            }
            db.BookmarkTitles.Remove(GetBookmarkTitle(username, tconst));
            db.SaveChanges();
            return true;
        }

        public bool DeleteBookmarkName(string username, string nconst)
        {
            //using var db = new ImdbContext();

            //var product = GetBookmarkName(username, nconst);

            //= 
            //var product = _db.BookmarkNames
            //    .FirstOrDefault(x => x.Username == username &&
            //    string.Compare(x.Nconst, nconst, CultureInfo.CurrentCulture,
            //        CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols)
            //    == 0
            //    )
            //    //== nconst.Trim())
            //    ;
            var product = _db.BookmarkNames.FirstOrDefault(x => x.Username == username && x.Nconst == nconst);
            Console.WriteLine("-" + product.Nconst + "-");
            
            if (product == null)
            {
                return false;
            }


            _db.BookmarkNames.Remove(product);
            _db.SaveChanges();
            return true;
        }

        public bool CreateUserRating(string username, string tconst, int rating)
        {
            using var db = new ImdbContext();

            var result = db.Database.ExecuteSqlInterpolated($"select * from user_rate({username}, {tconst}, {rating})");
            //Console.WriteLine(result);

            return true; // shouldn't return this - fix

        }

        public int CreateUserSearch(string username, string searchContent, string searchCategory = null)
        {

            using var db = new ImdbContext();
            var searchResults = db.UserSearches.FromSqlInterpolated($"SELECT * FROM save_string_search({username}, {searchContent}, {searchCategory})");
            var searchId = searchResults.FirstOrDefault().SearchId;

            //Console.WriteLine(searchResults.FirstOrDefault().SearchId);
            //Console.WriteLine(searchResults.FirstOrDefault().SearchId);
            //Console.WriteLine(searchId);
            //var searchResult = new SearchResult();
            //var titles = db.SearchTitleResults.FromSqlInterpolated($"select * from string_search_titles({searchContent})").ToList();
            //var names = db.SearchNameResults.FromSqlInterpolated($"select * from string_search_names({searchContent})").ToList();

            //var listTitles = GetFilteredTitles(titles);

            //searchResult.TitleResults = listTitles;


            return searchId;

        }

        /*
         
        public IList<ListTitleModelDL> GetTitlesForSearch(List<SearchTitleModel> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();

            //var filteredTitles = _db.FullViewTitles.ToList()
            var filteredTitles = db.FullViewTitles.ToList()
                .Join(searchedTitles,  //inner sequence
                    fullView => fullView.Tconst, //outerKeySelector 
                    searchResults => searchResults.Tconst,     //innerKeySelector
                    (fullView, searchResults)
                                  => fullView
                    )
                ;



            var groupedTitles = filteredTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new ListTitleModelDL
                {

                    BasicTitle = new BasicTitleModelDL
                    {
                        Tconst = model.First().Tconst,
                        PrimaryTitle = model.First().PrimaryTitle,
                        StartYear = model.First().StartYear,
                        TitleType = model.First().TitleType,
                    },
                    Runtime = model.First().Runtime,
                    Rating = model.First().Rating,
                    Genres = model.Select(m => m.Genre).Distinct().ToList(),
                    //ParentTitle = GetBasicTitle(model.FirstOrDefault().ParentTconst)
                    ParentTitle = string.IsNullOrEmpty(model.FirstOrDefault().ParentTconst) ? null : new DataService().GetBasicTitle(model.FirstOrDefault().ParentTconst)

                })




                .Skip(page * pageSize).Take(pageSize)
                .ToList();

            var listTitles2 = groupedTitles
                   //.GroupJoin(_db.TitleEpisodes,  //inner sequence
                   .GroupJoin(db.TitleEpisodes,  //inner sequence
                       std => std.BasicTitle.Tconst, //outerKeySelector 
                       s => s.Tconst,     //innerKeySelector
                       (std, s) =>
                       //std
                       new ListTitleModelDL
                       {

                           BasicTitle = new BasicTitleModelDL
                           {
                               Tconst = std.BasicTitle.Tconst,
                               PrimaryTitle = std.BasicTitle.PrimaryTitle,
                               StartYear = std.BasicTitle.StartYear,
                               TitleType = std.BasicTitle.TitleType,
                               //TitleType = x.TitleType,
                           },
                           Runtime = std.Runtime,
                           Rating = std.Rating,
                           Genres = std.Genres,
                       }
                       );




            //var listTitles3 = listTitles2.ToList().Select(
            //        x => x.ParentTitle = x.
            //    );


            return groupedTitles;
        }
         */




















    }
}