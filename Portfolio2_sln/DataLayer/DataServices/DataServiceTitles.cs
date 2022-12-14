using DataLayer.DataTransferObjects;
using DataLayer.DomainModels.TitleModels;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DataServices
{
    public class DataServiceTitles : IDataServiceTitles
    {

        private static ImdbContext _db = new ImdbContext();

        /* ------------
            TITLES
          ------------*/
        
        public int GetNumberOfTitles()
        {
            using var db = new ImdbContext();

            return db.TitleBasicss.Count();
        }


        public BasicTitleDTO GetBasicTitle(string tconst)
        {

            using var db = new ImdbContext();

            if (!db.TitleBasicss.Any(x => x.Tconst.Equals(tconst)))
                return null;

            var basicTitle = db.TitleBasicss
                .FirstOrDefault(x => x.Tconst.Equals(tconst));
            var basic = new BasicTitleDTO
            {
                Tconst = tconst,
                TitleType = basicTitle.TitleType,
                PrimaryTitle = basicTitle.PrimaryTitle,
                StartYear = basicTitle.StartYear,
            };
            return basic;
        }

        public IList<BasicTitleDTO> GetBasicTitles(int page = 0, int pageSize = 20)
        {
            Console.WriteLine(_db.TitleBasicss.FirstOrDefault().Tconst);
            var basicTitles = _db.TitleBasicss
                .Select(t => new BasicTitleDTO
                {
                    Tconst = t.Tconst,
                    TitleType = t.TitleType,
                    PrimaryTitle = t.PrimaryTitle,
                    StartYear = t.StartYear
                })
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return basicTitles;
        }

        public TitleForListDTO GetListTitle(string tconst)
        {
            using var db = new ImdbContext();

            if (!db.TitleBasicss.Any(x => x.Tconst.Equals(tconst)))
                return null;


            var fullTitle = db.FullViewTitles
                .Where(t => t.Tconst.Equals(tconst))
                .ToList();


            var listTitle = fullTitle
               .GroupBy(t => t.Tconst, (key, model) => new TitleForListDTO
               {
                   BasicTitle = new BasicTitleDTO
                   {
                       Tconst = model.First().Tconst,
                       PrimaryTitle = model.First().PrimaryTitle,
                       StartYear = model.First().StartYear,
                       TitleType = model.First().TitleType,
                   },
                   Runtime = model.First().Runtime,
                   Rating = model.First().Rating,
                   Genres = model.Select(m => m.Genre).Distinct().ToList(),
                   ParentTitle = string.IsNullOrEmpty(model.FirstOrDefault().ParentTconst)
                                   ? null
                                   : GetBasicTitle(model.FirstOrDefault().ParentTconst)
               })
           .FirstOrDefault();

            return listTitle;
        }

        public IList<TitleForListDTO> GetListTitles(int page = 1, int pageSize = 1)
        {
            using var db = new ImdbContext();

            // Just here to only perform next GroupBy with those within the given page
            var fullTitles = db.TitleBasicss
                .Skip((page - 1) * pageSize).Take(pageSize).ToList()
                .Join(db.FullViewTitles,
                    searchResults => searchResults.Tconst,
                    fullView => fullView.Tconst,
                    (searchResults, fullView)
                                  => fullView
                    );


            var listTitles = fullTitles.ToList()
               .GroupBy(t => t.Tconst, (key, model) => new TitleForListDTO
               {
                   BasicTitle = new BasicTitleDTO
                   {
                       Tconst = model.First().Tconst,
                       PrimaryTitle = model.First().PrimaryTitle,
                       StartYear = model.First().StartYear,
                       TitleType = model.First().TitleType,
                   },
                   Runtime = model.First().Runtime,
                   Rating = model.First().Rating,
                   Genres = model.Select(m => m.Genre).Distinct().ToList(),
                   ParentTitle = string.IsNullOrEmpty(model.FirstOrDefault().ParentTconst)
                                   ? null
                                   : GetBasicTitle(model.FirstOrDefault().ParentTconst)
               }).ToList();

            return listTitles;
        }


        public (int, IList<TitleCrewDTO>) GetTitleCrew(string tconst, int page, int pageSize)
        {
            using var db = new ImdbContext();

            var relatedNames = db.TitlePrincipals
                .Where(c => c.Tconst == tconst)
                .Join(db.NameBasicss,
                     crew => crew.Nconst,
                     nameBasics => nameBasics.Nconst,
                     (crew, nameBasics)
                     => new
                     {
                         Tconst = tconst,
                         Nconst = nameBasics.Nconst,
                         PrimaryName = nameBasics.PrimaryName,
                         Category = crew.Category
                     }
                    );

            var crew = relatedNames
                .GroupJoin(db.Jobs,
                     crew => new { crew.Nconst, crew.Tconst },
                     job => new { job.Nconst, job.Tconst },
                     (crew, job)
                     => new { Outer = crew, Inner = job })
                .SelectMany(
                x => x.Inner.DefaultIfEmpty(),
                (crew, job)
                    => new
                    {
                        Tconst = tconst,
                        Nconst = crew.Outer.Nconst,
                        PrimaryName = crew.Outer.PrimaryName,
                        Category = crew.Outer.Category,
                        JobName = job.JobName
                    }
             )
                .GroupJoin(db.Characters,
                     crew => new { crew.Nconst, crew.Tconst },
                     chars => new { chars.Nconst, chars.Tconst },
                     (crew, chars)
                     => new { Outer = crew, Inner = chars })
                .SelectMany(
                x => x.Inner.DefaultIfEmpty(),
                (crew, chars)
                    => new
                    TitleCrewDTO
                    {
                        Tconst = tconst,
                        Nconst = crew.Outer.Nconst,
                        PrimaryName = crew.Outer.PrimaryName,
                        Category = crew.Outer.Category,
                        JobName = crew.Outer.JobName,
                        CharacterName = chars.CharacterName
                    }

             )
            ;
            var totalItems = crew.Count();
            var result = crew
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return (totalItems, result);
        }

        public TvSeriesEpisodeDTO GetTvSeriesEpisode(string parentTconst, int seasonNumber, int episodeNumber)
        {
            using var db = new ImdbContext();

            var episode = db.TitleEpisodes
                .Where(p => p.ParentTconst.Equals(parentTconst)
                        && p.SeasonNumber == seasonNumber
                        && p.EpisodeNumber == episodeNumber
                ).ToList();


            var episodeResult = episode
            .Join(db.TitleBasicss,
                episodeTable => episodeTable.Tconst,
                titleBasicsTable => titleBasicsTable.Tconst,
                (episodeTable, titleBasicsTable)
                        => new TvSeriesEpisodeDTO
                        {
                            Tconst = episodeTable.Tconst,
                            PrimaryTitle = titleBasicsTable.PrimaryTitle,
                            ParentTconst = parentTconst,
                            EpisodeNumber = episodeTable.EpisodeNumber,
                            SeasonNumber = seasonNumber,
                        }
                ).FirstOrDefault();

            return episodeResult;
        }

        public TvSeriesSeasonDTO GetTvSeriesSeason(string tconst, int seasonNumber)
        {
            using var db = new ImdbContext();

            var filteredParentTitle = db.TitleEpisodes
                .Where(p => p.ParentTconst.Trim() == tconst.Trim())
                .Where(s => s.SeasonNumber == seasonNumber)
                .ToList();


            var filteredEpisodeTitles = filteredParentTitle
            .Join(db.TitleBasicss,
                episodeTable => episodeTable.Tconst,
                titleBasicsTable => titleBasicsTable.Tconst,
                (episodeTable, titleBasicsTable)
                        => new TvSeriesEpisodeDTO
                        {
                            SeasonNumber = seasonNumber,
                            Tconst = episodeTable.Tconst,
                            PrimaryTitle = titleBasicsTable.PrimaryTitle,
                            EpisodeNumber = episodeTable.EpisodeNumber,
                            ParentTconst = episodeTable.ParentTconst
                        }
                ).OrderBy(e => e.EpisodeNumber)
                .ToList();

            var result = new TvSeriesSeasonDTO
            {
                ParentTconst = tconst,
                SeasonNumber = seasonNumber,
                Episodes = filteredEpisodeTitles
            };

            return result;
        }


        public (int, IList<TvSeriesEpisodeDTO>) GetTvSeriesEpisodes(string tconst, int seasonNumber, int page, int pageSize)
        {
            using var db = new ImdbContext();

            var filteredParentTitle = db.TitleEpisodes
                .Where(p => p.ParentTconst.Equals(tconst))
                .Where(s => s.SeasonNumber == seasonNumber)
                .ToList();



            var filteredEpisodeTitles = filteredParentTitle
            .Join(db.TitleBasicss,
                episodeTable => episodeTable.Tconst,
                titleBasicsTable => titleBasicsTable.Tconst,
                (episodeTable, titleBasicsTable)
                        => new TvSeriesEpisodeDTO
                        {
                            SeasonNumber = seasonNumber,
                            Tconst = episodeTable.Tconst,
                            PrimaryTitle = titleBasicsTable.PrimaryTitle,
                            EpisodeNumber = episodeTable.EpisodeNumber,
                            ParentTconst = episodeTable.ParentTconst
                        }
                ).OrderBy(e => e.EpisodeNumber)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToList();

            int totalEpisodes = filteredParentTitle.Count();


            return (totalEpisodes, filteredEpisodeTitles);
        }

        /*
         
        public List<TvSeriesSeasonDTO> GetTvSeriesSeasons(string tconst)
        {
            using var db = new ImdbContext();

            var filteredParentTitle = db.TitleEpisodes
                .Where(t => t.ParentTconst.Equals(tconst))
                .ToList();

            var filteredEpisodeTitles = filteredParentTitle
                .Join(db.TitleBasicss,
                    episodeTable => episodeTable.Tconst,
                    titleBasicsTable => titleBasicsTable.Tconst,
                    (episodeTable, titleBasicsTable)
                            => new
                            {
                                EpisodeTconst = episodeTable.Tconst,
                                EpisodeTitle = titleBasicsTable.PrimaryTitle,
                                ParentTconst = episodeTable.ParentTconst,
                                SeasonNumber = episodeTable.SeasonNumber
                            }
                    );

            return null;
        }
         */



        public DetailedTitleDTO GetDetailedTitle(string tconst)
        {
            using var db = new ImdbContext();
            var filteredTitle = db.FullViewTitles.ToList()
                .Where(t => t.Tconst.Trim() == tconst.Trim())
                .ToList();


            var groupedTitle = filteredTitle.ToList()
                .GroupBy(t => t.Tconst, (key, model) =>
                new DetailedTitleDTO
                {
                    PrimaryTitle = model.First().PrimaryTitle,
                    StartYear = model.First().StartYear,
                    TitleType = model.First().TitleType,
                    Runtime = model.First().Runtime,
                    Rating = model.First().Rating,
                    Plot = model.First().Plot,
                    Poster = model.First().Poster,
                    Tconst = key,
                    Genres = model.Select(m => m.Genre).Distinct()
                        .ToList()
                }).FirstOrDefault();

            return groupedTitle;
        }



        public IList<TitleForListDTO> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 0, int pageSize = 5)
        {
            using var db = new ImdbContext();

            // Filters the FullViewTitles to only have those returned from the string search
            var filteredTitles = searchedTitles
                .Join(db.FullViewTitles,
                    searchResults => searchResults.Tconst,
                    fullView => fullView.Tconst,
                    (searchResults, fullView)
                                  => fullView
                    );

            // Groups the titles so we can make a list of genres for each title
            // and creates the list form DTO
            var groupedTitles = filteredTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new TitleForListDTO
                {
                    BasicTitle = new BasicTitleDTO
                    {
                        Tconst = model.First().Tconst,
                        PrimaryTitle = model.First().PrimaryTitle,
                        StartYear = model.First().StartYear,
                        TitleType = model.First().TitleType,
                    },
                    Runtime = model.First().Runtime,
                    Rating = model.First().Rating,
                    Genres = model.Select(m => m.Genre).Distinct().ToList(),
                    ParentTitle = string.IsNullOrEmpty(model.FirstOrDefault().ParentTconst)
                                    ? null
                                    : new DataServiceTitles().GetBasicTitle(model.FirstOrDefault().ParentTconst)
                })
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToList();


            return groupedTitles;
        }






    }
}