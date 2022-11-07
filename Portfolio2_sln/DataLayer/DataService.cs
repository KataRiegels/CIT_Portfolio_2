using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
            Console.WriteLine(tconst + "_DataService");
            var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == tconst);
            //var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == "tt9522300 ");
            Console.WriteLine(temp.Tconst + "_DataService2");
            return temp;
        }

        public IList<string> GetGenresFromTitle(string tconst)
        {
            //Console.WriteLine(tconst + "hello");
            //var genres = _db.TitleGenres
            //    .Where(x => x.Tconst.Contains(tconst))
            //    .ToList();
            Console.WriteLine(tconst + "_GetGenres");
            var genres =
                _db.TitleGenres.Where(x => x.Tconst.Contains(tconst.Trim()))
                //_db.TitleGenres.Where(x => x.Tconst == "tt9522300")
            .Select(x => x.GenreName)
            .ToList();

            Console.WriteLine(genres);

            //.ToString();

            //var genres2 = genres.Select(x => x.GenreName).ToList();
            //Console.WriteLine(genres.First().GenreName);
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

            //Console.WriteLine(innerJoin.First().TitleGenres.First().GenreName);
            Console.WriteLine("innerJoin length:" + innerJoin.Count());

            return innerJoin;
            //return null;
        }

    }
}