using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;

namespace DataLayer
{
    public interface IDataService
    {
        IList<TitleBasics>  GetTitles(string? titleType);
        TitleBasics GetTitle(string tconst);
        IList<TitlePrincipal> GetTitlesPrincipalFromName(string nconst);
        OmdbData GetOmdbData(string tconst);
        string GetPlot(string tconst);

        IList<NameBasics> GetNames();
        NameBasics GetName(string nconst);










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



    }
}
