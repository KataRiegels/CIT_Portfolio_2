using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace DataLayer
{
    public class DataService : IDataService
    {
        private static ImdbContext _db = new ImdbContext();
        public IList<TitleBasics> GetTitles()
        {
            return _db.TitleBasicss.ToList();
        }

        public TitleBasics GetTitle(string tconst)
        {
            
            var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == tconst);
            Console.WriteLine(temp);
            return temp;

            
        }
    }
}