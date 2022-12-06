using DataLayer.Model;
using DataLayer.Models.UserModels;

namespace DataLayer.DataServices
{
    public interface IDataServiceUser
    {
        void CreateBookmarkTitle(string username, string tconst, string annotation);
        void CreateUser(string username, string password, string birthYear, string email);
        bool CreateUserRating(string username, string tconst, int rating);
        SearchResult CreateUserSearch(string username, string searchContent, string searchCategory = null);
        bool DeleteBookmarkTitle(string username, string tconst);
        bool DeleteUser(string username);
        BookmarkTitle GetBookmarkTitle(string username, string tconst);
        IList<BookmarkTitle> GetBookmarkTitles();
        IList<BookmarkTitle> GetBookmarkTitlesByUser(string username);
        IList<ListTitleModelDL> GetTitlesForSearch(List<SearchTitleModel> searchedTitles, int page = 1, int pageSize = 5);
        User GetUser(string username);
        IList<User> GetUsers();
    }
}