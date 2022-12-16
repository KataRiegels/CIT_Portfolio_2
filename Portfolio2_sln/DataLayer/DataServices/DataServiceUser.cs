using DataLayer.DataTransferObjects;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DTOs.UserObjects;
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

        public void CreateUser(string username, string password, string email)
        {
            User newUser = new User()
            {
                Username = username,
                Password = password,
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

        //public BookmarkTitle GetBookmarkTitle(string username, string tconst)
        //{
        //    return _db.BookmarkTitles.FirstOrDefault(x => x.Username == username && x.Tconst.Trim() == tconst.Trim());
        //}


        public BookmarkName GetBookmarkName(string username, string nconst)
        {
            using var db = new ImdbContext();

            return db.BookmarkNames.FirstOrDefault(x => x.Username == username && x.Nconst.Trim() == nconst.Trim());
        }


        public IList<BookmarkTitle> GetBookmarkTitles()
        {
            using var db = new ImdbContext();

            return db.BookmarkTitles.ToList();
        }




        

        public IList<TitleForListDTO> GetFilteredTitles(List<BookmarkTitle> searchedTitles, int page = 1, int pageSize = 5)
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
                .GroupBy(t => t.Tconst, (key, model) => new TitleForListDTO
                {
                    BasicTitle = new BasicTitleDTO
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

        /*
         
         ------------- TITLE BOOKMARKS ---------------
         
         */

        public BookmarkTitle GetBookmarkTitle(string username, string tconst)
        {
            using var db = new ImdbContext();

            return db.BookmarkTitles.FirstOrDefault(bt => bt.Username.Equals(username) && bt.Tconst.Equals(tconst));

        }

        public IList<TitleForListDTO> GetBookmarkTitlesByUser(string username)
        {
            using var db = new ImdbContext();

            var bookmarksFilter = db.BookmarkTitles
                .Where(x => x.Username == username)
                .ToList();


            var result = new DataServiceTitles()
                .GetFilteredTitles(bookmarksFilter
                    .Select(x => new TconstObject { Tconst = x.Tconst }).ToList());

            return result;
        }



        public BookmarkTitle CreateBookmarkTitle(string username, string tconst)
        {
            using var db = new ImdbContext();


            var newBookmark = new BookmarkTitle
            {
                Username = username,
                Tconst = tconst,
            };

            db.BookmarkTitles.Add(newBookmark);
            db.SaveChanges();

            // Ensures that it is only returned if it was actually created
            var createdBookmark = GetBookmarkTitle(username, tconst);

            return createdBookmark;

        }

        public int DeleteBookmarkTitle(string username, string tconst)
        {
            using var db = new ImdbContext();

            var bookmarkToDelete = GetBookmarkTitle(username, tconst);

            // If the bookmark is not in the database
            if (bookmarkToDelete == null)
                return -1;

            db.BookmarkTitles.Remove(bookmarkToDelete);
            db.SaveChanges();

            // If the bookmark is still in the database
            if (GetBookmarkTitle(username, tconst) != null)
                return 0;

            // Bookmark was there + was removed
            return 1;
        }


        public BookmarkTitle UpdateBookmarkTitle(string username, string oldTconst, string newTconst)
        {
            using var db = new ImdbContext();

            var bookmarkToUpdate = GetBookmarkTitle(username, oldTconst);

            if (bookmarkToUpdate == null)
                return null;

            //bookmarkToUpdate.Username = username;
            bookmarkToUpdate.Tconst = newTconst;

            db.SaveChanges();

            // Ensures that it is only returned if it was actually created
            var updatedBookmark = GetBookmarkTitle(username, newTconst);

            return updatedBookmark;

        }


        /*
         
         ------------- Bookmark names --------------
         
         */

        //public BookmarkName GetBookmarkNameDM(string username, string nconst)
        //{
        //    using var db = new ImdbContext();

        //    return db.BookmarkNames.FirstOrDefault(bt => bt.Username.Equals(username) && bt.Nconst.Equals(nconst));

        //}


        public BookmarkName CreateBookmarkName(string username, string nconst)
        {
            using var db = new ImdbContext();

            var newBookmark = new BookmarkName()
            {
                Username = username,
                Nconst = nconst
            };

            db.BookmarkNames.Add(newBookmark);
            db.SaveChanges();

            var createdBookmark = GetBookmarkName(username, nconst);

            return createdBookmark;
        }

        public IList<NameForListDTO> GetBookmarkNamesByUser(string username)
        {
            using var db = new ImdbContext();

            var bookmarksFilter = db.BookmarkNames
                .Where(x => x.Username == username)
                .ToList();

            var result = new DataServiceNames()
                .GetFilteredNames(bookmarksFilter
                    .Select(x => new NconstObject { Nconst = x.Nconst }).ToList());

            return result;
        }

        public int DeleteBookmarkName(string username, string nconst)
        {
            using var db = new ImdbContext();

            var bookmarkToDelete = GetBookmarkName(username, nconst);

            // If the bookmark is not in the database
            if (bookmarkToDelete == null)
                return -1;

            // If the bookmark is still in the database

            db.BookmarkNames.Remove(bookmarkToDelete);
            db.SaveChanges();

            if (GetBookmarkName(username, nconst) != null)
                Console.WriteLine("still there");
                return 0;

            return 1;
        }


        


        public IList<UserRatingDTO> GetUserRatings(string username)
        {
            using var db = new ImdbContext();

            var ratings = db.UserRatings
                .Where(u => u.Username.Equals(username))
                .Join(db.TitleBasicss,
                    rating => rating.Tconst,
                    title => title.Tconst,
                    (rating, title)
                                  => new UserRatingDTO
                                  {
                                      Rating = rating.Rating,
                                      TitleModel = new BasicTitleDTO
                                      {
                                          Tconst = title.Tconst,
                                          StartYear = title.StartYear,
                                          PrimaryTitle = title.PrimaryTitle,
                                          TitleType = title.TitleType

                                      },
                                      Date = rating.Date
                                  }

                )
                .OrderBy(r => r.Date)
                .ToList();
            return ratings;

        }

        public bool DeleteUserRating(string username, string tconst)
        {
            var rating = GetUserRatings(username);
            if (rating == null)
            {
                return false;
            }
            _db.Users.Remove(GetUser(username));
            _db.SaveChanges();
            // check if rating is there

            return true;
        }


        public UserRating CreateUserRating(string username, string tconst, int rating)
        {
            using var db = new ImdbContext();

            db.Database.ExecuteSqlInterpolated($"select * from create_user_rating({username}, {tconst}, {rating})");

            var newUserRating = db.UserRatings.FirstOrDefault(u => u.Username == username && u.Tconst == tconst);

          
            return newUserRating; // shouldn't return this - fix
        }


        /*
         *
         *  --------- USER SEARCHES ------------
         *
         */ 


        public UserSearch GetUserSearch(int searchId)
        {
            using var db = new ImdbContext();

            return db.UserSearches.FirstOrDefault(u =>  u.SearchId == searchId);

        }


        public IList<UserSearch> GetUserSearches(string username)
        {

            using var db = new ImdbContext();

            return db.UserSearches.Where(u => u.Username.Equals(username)).ToList();


        }

        public UserSearch CreateUserSearch(string username, string searchContent, string searchCategory = null)
        {
            Console.WriteLine("----------------------DL1");

            using var db = new ImdbContext();
            
            var returnedCreatedUserSearch = db.UserSearches.FromSqlInterpolated($"SELECT * FROM save_string_search({username}, {searchContent}, {searchCategory})");
            Console.WriteLine("----------------------DL2");
            
            var createdSearchId = returnedCreatedUserSearch.FirstOrDefault().SearchId;
            Console.WriteLine("----------------------DL3");

            var createdUserSearch = db.UserSearches.FirstOrDefault(u => u.SearchId == createdSearchId);
            Console.WriteLine("----------------------DL4");

            return createdUserSearch;

        }

        public int DeleteUserSearch(int searchId)
        {
            var userSearchToDelete = GetUserSearch(searchId);
            if (userSearchToDelete == null)
            {
                return -1;
            }
            _db.UserSearches.Remove(userSearchToDelete);
            _db.SaveChanges();

            if (GetUserSearch(searchId) != null)
                return 0;

            // check if rating is there

            return 1;
        }


    }
}