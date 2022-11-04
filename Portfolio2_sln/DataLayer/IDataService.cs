using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models.TitleModels;

namespace DataLayer
{
    public interface IDataService
    {
        IList<TitleBasics> GetTitles();
        TitleBasics GetTitle(string tconst);

    }
}
