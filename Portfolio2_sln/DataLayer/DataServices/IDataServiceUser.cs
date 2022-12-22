using DataLayer.DomainModels.UserModels;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DTOs.UserObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceUser
    {
        User CreateUser(string username, string password, string email);
        bool DeleteUser(string username);
        User GetUser(string username);
        IList<User> GetUsers();





        public (int, IList<UserRatingDTO>) GetUserRatings(string username, int page, int pageSize);
        public bool DeleteUserRating(string username, string tconst);
        UserRatingDTO CreateUserRating(string username, string tconst, int rating);
        public UserRatingDTO GetUserRating(string username, string tconst);
        public UserRatingDTO UpdateUserRating(string username, string tconst, int rating);




        public int DeleteBookmarkName(string username, string nconst);

        public BookmarkName GetBookmarkName(string username, string nconst);
        public BookmarkName CreateBookmarkName(string username, string nconst);

        public (int, IList<NameForListDTO>) GetBookmarkNamesByUser(string username, int page, int pageSize);




        BookmarkTitle CreateBookmarkTitle(string username, string tconst);
        BookmarkTitle UpdateBookmarkTitle(string username, string oldTconst, string newTconst);
        int DeleteBookmarkTitle(string username, string tconst);
        BookmarkTitle GetBookmarkTitle(string username, string tconst);
        (int, IList<TitleForListDTO>) GetBookmarkTitlesByUser(string username, int page, int pageSize);




        UserSearch CreateUserSearch(string username, string searchContent, string searchCategory = null);
        public (int, IList<UserSearch>) GetUserSearches(string username, int page, int pageSize);

        public UserSearch GetUserSearch(int searchId);
        int DeleteUserSearch(int searchId);






    }
}