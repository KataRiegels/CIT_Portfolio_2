using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;

namespace DataLayer
{
    public interface IDataService
    {
        IList<TitleBasics>  GetTitles(string? titleType);
        TitleBasics         GetTitle(string tconst);

        IList<NameBasics> GetNames();
        NameBasics GetName(string nconst);


        IList<TitleBasics> GetTitlesByGenre(string genreName);

        // Make private? or in Controller, maybe?
        public IList<string> GetGenresFromTitle(string tconst);
        public IList<TitleBasics> GetEpisodesFromTitle(string parentTconst);



    }
}
