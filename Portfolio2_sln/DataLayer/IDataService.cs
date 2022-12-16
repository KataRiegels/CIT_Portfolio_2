using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DTOs.SearchObjects;

namespace DataLayer
{
    public interface IDataService
    {
        void CreateBookmarkTitle(string username, string tconst, string annotation);
        void CreateUser(string username, string password, string email);
        bool CreateUserRating(string username, string tconst, int rating);
        SearchResultDTO CreateUserSearch(string username, string searchContent, string searchCategory = null);
        bool DeleteBookmarkTitle(string username, string tconst);
        bool DeleteUser(string username);
        BasicNameDTO GetBasicName(string nconst);
        IList<BasicNameDTO> GetBasicNames(int page = 0, int pageSize = 20);
        BasicTitleDTO GetBasicTitle(string tconst);
        IList<BasicTitleDTO> GetBasicTitles(int page = 0, int pageSize = 20);
        BookmarkTitle GetBookmarkTitle(string username, string tconst);
        IList<BookmarkTitle> GetBookmarkTitles();
        IList<BookmarkTitle> GetBookmarkTitlesByUser(string username);
        IList<DetailedNameDTO>? GetDetailedNames(int page = 0, int pageSize = 20);
        DetailedTitleDTO GetDetailedTitle(string tconst);
        IList<DetailedTitleDTO>? GetDetailedTitles(int page, int pageSize);
        IList<NameForListDTO> GetListNames(int page = 0, int pageSize = 20);
        TitleForListDTO GetListTitle(string tconst);
        IList<TitleForListDTO> GetListTitles(int page, int pageSize);
        NameBasics GetName(string nconst);
        IList<NameBasics> GetNames(int page = 0, int pageSize = 20);
        TitleBasics GetTitle(string tconst);
        IList<TitleBasics> GetTitles(int page = 0, int pageSize = 20);
        User GetUser(string username);
        IList<User> GetUsers();

    }
}

