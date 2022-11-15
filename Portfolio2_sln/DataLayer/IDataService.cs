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
        IList<TitleBasics>  GetTitles(string? titleType, int page, int pageSize);
        TitleBasics GetTitle(string tconst);
        IList<TitlePrincipal> GetTitlesPrincipalFromName(string nconst, int page, int pageSize);
        OmdbData GetOmdbData(string tconst);
        string GetPlot(string tconst);

        IList<NameBasics> GetNames(int page, int pageSize);
        NameBasics GetName(string nconst);

        IList<DetailedNameModelDL> GetDetailedNames(int page, int pageSize);

        public User GetUser(string username);
        public IList<User> GetUsers(int page, int pageSize);

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
        public IList<BasicNameModelDL> GetBasicNames(int page, int pageSize);
        public IList<ListNameModelDL> GetListNames(int page, int pageSize);
        public IList<DetailedActorModel> GetDetailedActors(int page, int pageSize);
        public IList<DetailedProducerModel> GetDetailedProducers(int page, int pageSize);
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

        public IList<BasicTitleModelDL> GetBasicTitles(int page, int pageSize);
        public IList<ListTitleModelDL> GetListTitles(int page, int pageSize);
        public IList<DetailedTitleModelDL> GetDetailedTitles(int page, int pageSize);
        public BasicTitleModelDL GetBasicTitle(string tconst);
        public ListTitleModelDL GetListTitle(string tconst);
        public DetailedTitleModelDL GetDetailedTitle(string tconst);

        // Make private? or in Controller, maybe?
        //public IList<string> GetGenresFromTitle(string tconst);
        public IList<TitleBasics> GetEpisodesFromTitle(string parentTconst);



        public IList<BookmarkTitleTest> GetTitleBookmarks();

    }
}
