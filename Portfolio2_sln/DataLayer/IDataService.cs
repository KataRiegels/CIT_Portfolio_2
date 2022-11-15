using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;

namespace DataLayer
{
    public interface IDataService
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
        IList<TitleBasics> GetTitles(int page = 0, int pageSize = 20);
        User GetUser(string username);
        IList<User> GetUsers();

        IList<DetailedNameModelDL> GetDetailedNames();

        public User GetUser(string username);
        public IList<User> GetUsers();

        public void CreateUser(string username, string password, string birthYear, string email);

        public bool DeleteUser(string username);

        public BookmarkTitle GetBookmarkTitle(string username, string tconst);
        public IList<BookmarkTitle> GetBookmarkTitles();

        public IList<BookmarkTitle> GetBookmarkTitlesByUser(string username);

        public void CreateBookmarkTitle(string username, string tconst, string annotation);

        public bool DeleteBookmarkTitle(string username, string tconst);

        //---------------------------------------------------------------------------------------------
        //                    NAME
        //---------------------------------------------------------------------------------------------
        public IList<BasicNameModelDL> GetBasicNames();
        public IList<ListNameModelDL> GetListNames();
        public IList<DetailedActorModel> GetDetailedActors();
        public IList<DetailedProducerModel> GetDetailedProducers();
        //public BasicNameModelDL GetBasicName(string nconst);
        //public DetailedActorModel GetDetailedActor(string nconst);
        //public DetailedProducerModel GetDetailedProducer(string nconst);

        //---------------------------------------------------------------------------------------------
        //             NAME HELPERS
        //---------------------------------------------------------------------------------------------

        public string GetProfession(string nconst);
        public string GetKnownFor(string nconst);



        //public static double GetRatingFromTitle(string tconst);

        // TITLES
        public IList<TitleAka> GetTitleAkasByTitle(string tconst);
        //IList<TitleBasics> GetTitlesByGenre(string genreName);

        public IList<BasicTitleModelDL> GetBasicTitles();
        public IList<ListTitleModelDL> GetListTitles();
        public IList<DetailedTitleModelDL> GetDetailedTitles();
        public BasicTitleModelDL GetBasicTitle(string tconst);
        public ListTitleModelDL GetListTitle(string tconst);
        public DetailedTitleModelDL GetDetailedTitle(string tconst);

        // Make private? or in Controller, maybe?
        //public IList<string> GetGenresFromTitle(string tconst);
        public IList<TitleBasics> GetEpisodesFromTitle(string parentTconst);



        public IList<BookmarkTitleTest> GetTitleBookmarks();

        public bool CreateUserRating(string username, string tconst, int rating);

        public SearchResult CreateUserSearch(string username, string searchContent, string seachCategory = null);


    }
}

