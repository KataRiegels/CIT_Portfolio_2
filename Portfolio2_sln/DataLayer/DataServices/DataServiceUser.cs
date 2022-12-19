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

        public User CreateUser(string username, string password, string email)
        {
            User newUser = new User()
            {
                Username = username,
                Password = password,
                Email = email
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return GetUser(username);

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

            return db.BookmarkNames.FirstOrDefault(x => x.Username == username && x.Nconst.Equals(nconst));
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

        public (int, IList<TitleForListDTO>) GetBookmarkTitlesByUser(string username, int page, int pageSize)
        {
            using var db = new ImdbContext();

            var bookmarksFilter = db.BookmarkTitles
                .Where(x => x.Username == username)
                .ToList();
            var totalItems = bookmarksFilter.Count();

            var result = new DataServiceTitles()
                .GetFilteredTitles(bookmarksFilter
                    .Select(x => new TconstObject { Tconst = x.Tconst }).ToList())
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return (totalItems, result);
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

        public (int, IList<NameForListDTO>) GetBookmarkNamesByUser(string username, int page = 1, int pageSize = 20)
        {
            using var db = new ImdbContext();

            var bookmarksFilter = db.BookmarkNames
                .Where(x => x.Username == username)
                .ToList();
            var totalItems = bookmarksFilter.Count();

            var result = new DataServiceNames()
                .GetFilteredNames(bookmarksFilter
                    .Select(x => new NconstObject { Nconst = x.Nconst }).ToList())
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (totalItems, result);
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
            {
                return 0;
            }

            return 1;
        }


        
        /*
         
         
         User rating
         
         */


        public UserRatingDTO GetUserRating(string username, string tconst)
        {
            using var db = new ImdbContext();

            var rating = db.UserRatings
                .Where(u => u.Username.Equals(username) && u.Tconst.Equals(tconst))
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
                .FirstOrDefault();

            return rating;
        }


        public (int, IList<UserRatingDTO>) GetUserRatings(string username, int page, int pageSize)
        {
            using var db = new ImdbContext();

            var ratings = db.UserRatings
                .Where(u => u.Username.Equals(username));

            var totalItems = ratings.Count();

            var result = ratings
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
                .Skip((page - 1) * pageSize).Take(pageSize).ToList()
                .OrderBy(r => r.Date)
                .ToList();

            return (totalItems, result);

        }

        public bool DeleteUserRating(string username, string tconst)
        {
            using var db = new ImdbContext();


            var rating = db.UserRatings.FirstOrDefault(u => u.Username.Equals(username) && u.Tconst.Equals(tconst));

            if (rating == null)
                return false;

            db.UserRatings.Remove(rating);
            db.SaveChanges();
            // check if rating is there

            return true;
        }

        // Currently just deletes and creates due to making sure SQL also updates rating rate
        public UserRating UpdateUserRating(string username, string tconst, int rating)
        {
            using var db = new ImdbContext();

            var ratingToUpdate = db.UserRatings.FirstOrDefault(u => u.Username.Equals(username) && u.Tconst.Equals(tconst));

            if (ratingToUpdate == null)
                return null;

            DeleteUserRating(username, tconst);
            db.SaveChanges();

            CreateUserRating(username, tconst, rating);
            db.SaveChanges();

            var newUserRating = db.UserRatings.FirstOrDefault(u => u.Username == username && u.Tconst == tconst);

            return newUserRating; // shouldn't return this - fix
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


        public (int, IList<UserSearch>) GetUserSearches(string username, int page = 0, int pageSize = 10)
        {

            using var db = new ImdbContext();
            var userSearches = db.UserSearches.Where(u => u.Username.Equals(username));
            var totalItems = userSearches.Count();
            var result = userSearches
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return (totalItems, result);


        }

        public UserSearch CreateUserSearch(string username, string searchContent, string searchCategory = null)
        {

            using var db = new ImdbContext();
            
            var returnedCreatedUserSearch = db.UserSearches.FromSqlInterpolated($"SELECT * FROM save_string_search({username}, {searchContent}, {searchCategory})");
            
            var createdSearchId = returnedCreatedUserSearch.FirstOrDefault().SearchId;

            var createdUserSearch = db.UserSearches.FirstOrDefault(u => u.SearchId == createdSearchId);

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