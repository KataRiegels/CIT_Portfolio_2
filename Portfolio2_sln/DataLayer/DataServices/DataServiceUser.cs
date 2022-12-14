using DataLayer.DataTransferObjects;
using DataLayer.DomainModels.UserModels;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DTOs.UserObjects;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices
{
    public class DataServiceUser : IDataServiceUser
    {


        /* ----------------------------------------------------------------------------------------------------------
                        USER 
        ---------------------------------------------------------------------------------------------------------- */

        public User GetUser(string username)
        {
            using var db = new ImdbContext();

            if (!db.Users.Any(x => x.Username == username))
            {
                return null;
            }

            return db.Users.FirstOrDefault(x => x.Username == username);
        }

        public IList<User> GetUsers()
        {
            using var db = new ImdbContext();

            return db.Users.ToList();
        }

        public User CreateUser(string username, string password, string email)
        {
            using var db = new ImdbContext();

            User newUser = new User()
            {
                Username = username,
                Password = password,
                Email = email
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return GetUser(username);

        }

        public bool DeleteUser(string username)
        {
            using var db = new ImdbContext();


            var product = GetUser(username);
            if (product == null)
            {
                return false;
            }
            db.Users.Remove(GetUser(username));
            db.SaveChanges();
            return true;
        }

        /*
         
         BOOKMARK TITLES
         */

        public BookmarkName GetBookmarkName(string username, string nconst)
        {
            using var db = new ImdbContext();

            return db.BookmarkNames.FirstOrDefault(x => x.Username == username && x.Nconst.Equals(nconst));
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

            bookmarkToUpdate.Tconst = newTconst;

            db.SaveChanges();

            // Ensures that it is only returned if it was actually created
            var updatedBookmark = GetBookmarkTitle(username, newTconst);

            return updatedBookmark;

        }


        /*
         
         ------------- Bookmark names --------------
         
         */


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

            return true;
        }

        // Currently just deletes and creates due to making sure SQL also updates rating rate
        public UserRatingDTO UpdateUserRating(string username, string tconst, int rating)
        {
            using var db = new ImdbContext();

            var checkIfExists = db.UserRatings.Any(u => u.Username.Equals(username) && u.Tconst.Equals(tconst));

            if (checkIfExists == false)
            {

                Console.WriteLine("fuck");
                return null;
            }


            var ratingToUpdate = db.UserRatings.FirstOrDefault(u => u.Username.Equals(username) && u.Tconst.Equals(tconst));


            DeleteUserRating(username, tconst);
            db.SaveChanges();

            CreateUserRating(username, tconst, rating);
            db.SaveChanges();

            var newUserRating = GetUserRating(username, tconst);

            return newUserRating;
        }

        public UserRatingDTO CreateUserRating(string username, string tconst, int rating)
        {
            using var db = new ImdbContext();

            db.Database.ExecuteSqlInterpolated($"select * from create_user_rating({username}, {tconst}, {rating})");

            var newUserRating = GetUserRating(username, tconst);


            return newUserRating;
        }


        /*
         *
         *  --------- USER SEARCHES ------------
         *
         */


        public UserSearch GetUserSearch(int searchId)
        {
            using var db = new ImdbContext();

            return db.UserSearches.FirstOrDefault(u => u.SearchId == searchId);

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
            using var db = new ImdbContext();

            var userSearchToDelete = GetUserSearch(searchId);
            if (userSearchToDelete == null)
            {
                return -1;
            }
            db.UserSearches.Remove(userSearchToDelete);
            db.SaveChanges();

            if (GetUserSearch(searchId) != null)
                return 0;


            return 1;
        }


    }
}