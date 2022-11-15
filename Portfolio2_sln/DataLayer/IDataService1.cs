using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;

namespace DataLayer
{
    public interface IDataService1
    {
        void CreateBookmarkTitle(string username, string tconst, string annotation);
        void CreateUser(string username, string password, string birthYear, string email);
        bool DeleteBookmarkTitle(string username, string tconst);
        bool DeleteUser(string username);
        BasicNameModelDL GetBasicName(string nconst);
        IList<BasicNameModelDL> GetBasicNames(int page = 0, int pageSize = 20);
        BasicTitleModelDL GetBasicTitle(string tconst);
        IList<BasicTitleModelDL> GetBasicTitles(int page = 0, int pageSize = 20);
        BookmarkTitle GetBookmarkTitle(string username, string tconst);
        IList<BookmarkTitle> GetBookmarkTitles();
        IList<BookmarkTitle> GetBookmarkTitlesByUser(string username);
        IList<DetailedNameModelDL>? GetDetailedNames(int page = 0, int pageSize = 20);
        DetailedTitleModelDL GetDetailedTitle(string tconst);
        IList<DetailedTitleModelDL>? GetDetailedTitles(int page, int pageSize);
        IList<ListNameModelDL> GetListNames(int page = 0, int pageSize = 20);
        ListTitleModelDL GetListTitle(string tconst);
        IList<ListTitleModelDL> GetListTitles(int page, int pageSize);
        NameBasics GetName(string nconst);
        IList<NameBasics> GetNames(int page = 0, int pageSize = 20);
        TitleBasics GetTitle(string tconst);
        IList<TitleBasics> GetTitles(string? titleType = null);
        User GetUser(string username);
        IList<User> GetUsers();
    }
}