using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;

namespace DataLayer
{
    public class DataService : IDataService
    {
        private static ImdbContext _db = new ImdbContext();

        /* ------------
            TITLES
          ------------*/
        // can currently get with one query
        public IList<TitleBasics> GetTitles(string? titleType = null)
        {
            Console.WriteLine(titleType);
            var result = _db.TitleBasicss.ToList();
            if (titleType != null)
            {
                result = _db.TitleBasicss.Where(x => x.TitleType == (titleType)).ToList();
                Console.WriteLine(result.Count());
            }

            return result;
        }

        public TitleBasics GetTitle(string tconst)
        {
            var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == tconst);
            
            return temp;
        }

        public IList<TitleAka> GetTitleAkasByTitle(string tconst)
        {
            return _db.TitleAkas.Where(x => x.Tconst == tconst).ToList(); ;
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


            var innerJoin = titleGenres.Join(
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





        public IList<TitleBasics> GetEpisodesFromTitle(string parentTconst)
        {
            Console.WriteLine(parentTconst);
            var episodes = _db.TitleEpisodes
                .Where(e => e.ParentTconst == parentTconst.Trim())
                //.Where(e => e.ParentTconst == parentTconst)
                .ToList();
            foreach (var episode in episodes)
            {
                Console.WriteLine(episode.Tconst);
            }
            Console.WriteLine(episodes.Count());

            var innerJoin = episodes.Join(
                    _db.TitleBasicss,
                    episode => episode.Tconst,
                    title => title.Tconst,
                    (episode, title) =>
                    title
                   
                    )
                    .ToList();
            Console.WriteLine(episodes.Count());
            Console.WriteLine(innerJoin.Count());

            return innerJoin;
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