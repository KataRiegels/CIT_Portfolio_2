using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Runtime.CompilerServices;

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
            return temp;
        }

        public IList<string> GetGenresFromTitle(string tconst)
        {
            var genres =
                _db.TitleGenres.Where(x => x.Tconst.Contains(tconst.Trim()))
            .Select(x => x.GenreName)
            .ToList();

            return genres;
        }

        public IList<TitleBasics> GetTitlesByGenre(string genreName)
        {
            IList<TitleGenre> titleGenres =
                _db.TitleGenres.Where(x => x.GenreName.Contains(genreName)).ToList();


            var innerJoin = _db.TitleGenres.Join(
                    _db.TitleBasicss,
                    genre => genre.Tconst,
                    title => title.Tconst,
                    (genre, title) => new TitleBasics
                    {
                        Tconst = title.Tconst,
                        TitleType = title.TitleType,
                        PrimaryTitle = title.PrimaryTitle,
                        OriginalTitle = title.OriginalTitle,
                        IsAdult = title.IsAdult,
                        StartYear = title.StartYear,
                        EndYear = title.EndYear,
                        RunTimeMinutes = title.RunTimeMinutes
                        //TitleGenres = new List<TitleGenre>() { genre }
                    }
                    )
                    .ToList();


            return innerJoin;
            //return null;
        }

        public NameBasics GetName(string nconst) 
        {
            var temp = _db.NameBasicss.FirstOrDefault(x => x.Nconst == nconst);
            return temp;
        }

        public IList<NameBasics> GetNames() 
        {
            return _db.NameBasicss.ToList();
        }

    }
}