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

        IList<NameBasics> GetNames();
        NameBasics GetName(string nconst);

        IList<TitleBasics> GetTitles();
        TitleBasics GetTitle(string tconst);

        IList<TitleBasics> GetTitlesByGenre(string genreName);

        public IList<string> GetGenresFromTitle(string tconst);
    }
}
