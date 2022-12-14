using DataLayer.DataTransferObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataLayer.DataServices
{
    public class DataServiceTitles : IDataServiceTitles
    {

        private static ImdbContext _db = new ImdbContext();

        /* ------------
            TITLES
          ------------*/
        public IList<TitleBasics> GetTitles(int page = 0, int pageSize = 20)
        {
            var result = _db.TitleBasicss.Skip(page * pageSize).Take(pageSize).ToList();
            result = _db.TitleBasicss.Skip(page * pageSize).Take(pageSize).ToList();

            return result;
        }

        public int GetNumberOfTitles()
        {
            return _db.TitleBasicss.Count();
        }

        public TitleBasics GetTitle(string tconst)
        {
            var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == tconst);

            return temp;
        }

        public BasicTitleDTO GetBasicTitle(string tconst)
        {
            using var db = new ImdbContext();
            //var basicTitle = _db.TitleBasicss
            var basicTitle = db.TitleBasicss
                .FirstOrDefault(x => x.Tconst.Trim() == tconst.Trim());
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

            Console.WriteLine(_db.TitleBasicss.First().Tconst);
            var basicTitles = _db.TitleBasicss
                .Select(t => new BasicTitleDTO
                {
                    Tconst = t.Tconst,
                    TitleType = t.TitleType,
                    PrimaryTitle = t.PrimaryTitle,
                    StartYear = t.StartYear
                })
                .Skip(page * pageSize).Take(pageSize).ToList();

            return basicTitles;
        }

     
        public IList<TitleForListDTO> GetListTitles(int page = 0, int pageSize = 1)
        {
            using var db = new ImdbContext();

            //var titles = _db.FullViewTitles

            //    .ToList()
            //    .GroupBy(t => t.Tconst, (key, model) => new TitleForListDTO
            //    {

            //        BasicTitle = new BasicTitleDTO
            //        {
            //            Tconst = model.First().Tconst,
            //            PrimaryTitle = model.First().PrimaryTitle,
            //            StartYear = model.First().StartYear,
            //            TitleType = model.First().TitleType
            //        },
            //    }).Skip(page * pageSize).Take(pageSize).ToList();

            var result = db.TitleBasicss
                .Skip(page * pageSize).Take(pageSize).ToList()
                .Join(db.FullViewTitles,
                    searchResults => searchResults.Tconst,
                    fullView => fullView.Tconst,
                    (searchResults, fullView)
                                  => fullView
                    );

            ;



            //var titles = _db.FullViewTitles
            var titles = result

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
                               : GetBasicTitle(model.FirstOrDefault().ParentTconst)
           })
           //.Skip(page * pageSize).Take(pageSize)
           .ToList();

            return titles;
        }




        public IList<DetailedTitleDTO>? GetDetailedTitles(int page, int pageSize)
        {

            using var db = new ImdbContext();

            var titles = db.FullViewTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new DetailedTitleDTO
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
                    .Skip(page * pageSize).Take(pageSize).ToList()
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();



            var temp = new List<DetailedTitleDTO>();
            return titles;
        }

        public IList<TitleCastDTO> GetTitleCast(string tconst)
        {
            using var db = new ImdbContext();


            var cast = db.Characters
                .Where(c => c.Tconst.Trim() == tconst.Trim())
                .Join(db.NameBasicss,
                     chars => chars.Nconst,
                     nameBasics => nameBasics.Nconst,
                     (chars, nameBasics)
                             => new TitleCastDTO
                             {
                                 Tconst = tconst,
                                 Nconst = nameBasics.Nconst,
                                 PrimaryName = nameBasics.PrimaryName,
                                 CharacterName = chars.CharacterName
                             }
             )
             .ToList();

            return cast;
        }

        public IList<TitleCrewDTO> GetTitleCrew(string tconst)
        {
            using var db = new ImdbContext();

            var cast1 = db.TitlePrincipals
                .Where(c => c.Tconst.Trim() == tconst.Trim())
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

            var cast = cast1
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
                                 Tconst         = tconst,
                                 Nconst         = crew.Outer.Nconst,
                                 PrimaryName    = crew.Outer.PrimaryName,
                                 Category       = crew.Outer.Category,
                                 JobName        = crew.Outer.JobName,
                                 CharacterName  = chars.CharacterName
                             }

             )
            .ToList();


            return cast;
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
                            Tconst = episodeTable.Tconst,
                            PrimaryTitle = titleBasicsTable.PrimaryTitle,
                            EpisodeNumber = episodeTable.EpisodeNumber,
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

            public List<TvSeriesSeasonDTO> GetTvSeriesSeasons(string tconst)
        {
            using var db = new ImdbContext();

            var filteredParentTitle = db.TitleEpisodes
                .Where(t => t.ParentTconst.Trim() == tconst.Trim())
                .ToList();

            var filteredEpisodeTitles = filteredParentTitle
                .Join(db.TitleBasicss,
                    episodeTable => episodeTable.Tconst, 
                    titleBasicsTable => titleBasicsTable.Tconst,  
                    (episodeTable, titleBasicsTable)
                            => new { 
                                      EpisodeTconst = episodeTable.Tconst,
                                    EpisodeTitle = titleBasicsTable.PrimaryTitle,      
                                    ParentTconst  = episodeTable.ParentTconst,
                                    SeasonNumber = episodeTable.SeasonNumber
                            }
                    );


            /*
             
             
            var seasons = filteredEpisodeTitles
                .GroupBy(t => t.SeasonNumber, (key, model) =>
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
                }
                )
                ;
             */

            return null;



            //var seasons = 


        }




        public TitleForListDTO GetListTitle(string tconst)
        {
            return null;
        }

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



        public IList<TitleForListDTO> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();

            // Filters the FullViewTitles to only have those returned from the string search
            var filteredTitles = db.FullViewTitles.ToList()
                .Join(searchedTitles,
                    fullView => fullView.Tconst,
                    searchResults => searchResults.Tconst,
                    (fullView, searchResults)
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
                                    : new DataService().GetBasicTitle(model.FirstOrDefault().ParentTconst)
                })
                .Skip(page * pageSize).Take(pageSize)
                .ToList();


            return groupedTitles;
        }












    }
}