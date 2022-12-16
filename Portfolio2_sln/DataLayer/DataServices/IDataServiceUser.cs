using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DTOs.UserObjects;
using DataLayer.Models.UserModels;

namespace DataLayer.DataServices
{
    public interface IDataServiceUser
    {
        public IList<UserSearch> GetUserSearches(string username);

        public bool DeleteUserRating(string username, string tconst);

        public IList<UserRatingDTO> GetUserRatings(string username);
        public int DeleteBookmarkName(string username, string nconst);

        public BookmarkName GetBookmarkName(string username, string nconst);


        public IList<NameForListDTO> GetBookmarkNamesByUser(string username);
        BookmarkTitle CreateBookmarkTitle(string username, string tconst);
        void CreateUser(string username, string password, string email);
        UserSearch CreateUserSearch(string username, string searchContent, string searchCategory = null);
        int DeleteBookmarkTitle(string username, string tconst);


        bool DeleteUser(string username);
        BookmarkTitle GetBookmarkTitle(string username, string tconst);
        IList<BookmarkTitle> GetBookmarkTitles();
        IList<TitleForListDTO> GetBookmarkTitlesByUser(string username);
        //IList<TitleForListDTO> GetTitlesForSearch(List<TitleSearchResult> searchedTitles, int page = 1, int pageSize = 5);
        User GetUser(string username);
        IList<User> GetUsers();
        UserRating CreateUserRating(string username, string tconst, int rating);

        public BookmarkName CreateBookmarkName(string username, string nconst);

        public UserSearch GetUserSearch(int searchId);
        int DeleteUserSearch(int searchId);
        BookmarkTitle UpdateBookmarkTitle(string username, string oldTconst, string newTconst);
    }
}