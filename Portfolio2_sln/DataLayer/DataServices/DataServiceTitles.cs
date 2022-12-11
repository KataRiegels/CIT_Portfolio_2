﻿using DataLayer.DataTransferObjects;
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

        public TitleBasics GetTitle(string tconst)
        {
            var temp = _db.TitleBasicss.FirstOrDefault(x => x.Tconst == tconst);

            return temp;
        }

        public BasicTitleModelDL GetBasicTitle(string tconst)
        {
            using var db = new ImdbContext();
            //var basicTitle = _db.TitleBasicss
            var basicTitle = db.TitleBasicss
                .FirstOrDefault(x => x.Tconst.Trim() == tconst.Trim());
            var basic = new BasicTitleModelDL
            {
                Tconst = tconst,
                TitleType = basicTitle.TitleType,
                PrimaryTitle = basicTitle.PrimaryTitle,
                StartYear = basicTitle.StartYear,
            };

            return basic;
        }

        public IList<BasicTitleModelDL> GetBasicTitles(int page = 0, int pageSize = 20)
        {

            Console.WriteLine(_db.TitleBasicss.First().Tconst);
            var basicTitles = _db.TitleBasicss
                .Select(t => new BasicTitleModelDL
                {
                    Tconst = t.Tconst,
                    TitleType = t.TitleType,
                    PrimaryTitle = t.PrimaryTitle,
                    StartYear = t.StartYear
                })
                .Skip(page * pageSize).Take(pageSize).ToList();

            return basicTitles;
        }


        public IList<ListTitleModelDL> GetListTitles(int page = 0, int pageSize = 1)
        {

            var titles = _db.FullViewTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new ListTitleModelDL
                {

                    BasicTitle = new BasicTitleModelDL
                    {
                        Tconst = model.First().Tconst,
                        PrimaryTitle = model.First().PrimaryTitle,
                        StartYear = model.First().StartYear,
                        TitleType = model.First().TitleType
                    },
                }).Skip(page * pageSize).Take(pageSize).ToList();

            return titles;
        }




        public IList<DetailedTitleModelDL>? GetDetailedTitles(int page, int pageSize)
        {

            using var db = new ImdbContext();

            var titles = db.FullViewTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new DetailedTitleModelDL
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



            var temp = new List<DetailedTitleModelDL>();
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

        public IList<TitleCastDTO> GetTitleCrew(string tconst)
        {
            using var db = new ImdbContext();


            var cast = db.Jobs
                .Where(c => c.Tconst.Trim() == tconst.Trim())
                .Join(db.NameBasicss,
                     crew => crew.Nconst,
                     nameBasics => nameBasics.Nconst,
                     (crew, nameBasics)
                             => new TitleCastDTO
                             {
                                 Tconst = tconst,
                                 Nconst = nameBasics.Nconst,
                                 PrimaryName = nameBasics.PrimaryName,
                                 CharacterName = crew.JobName
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
                new DetailedTitleModelDL
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




        public ListTitleModelDL GetListTitle(string tconst)
        {
            return null;
        }

        public DetailedTitleModelDL GetDetailedTitle(string tconst)
        {
            using var db = new ImdbContext();
            var filteredTitle = db.FullViewTitles.ToList()
                .Where(t => t.Tconst.Trim() == tconst.Trim())
                .ToList();


            var groupedTitle = filteredTitle.ToList()
                .GroupBy(t => t.Tconst, (key, model) =>
                new DetailedTitleModelDL
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
            //var titles = _db.FullViewTitles

            //    .ToList()
            //    .GroupBy(t => t.Tconst, (key, model) => new DetailedTitleModelDL
            //    {
            //        PrimaryTitle = model.First().PrimaryTitle,
            //        StartYear = model.First().StartYear,
            //        TitleType = model.First().TitleType,
            //        Runtime = model.First().Runtime,
            //        Rating = model.First().Rating,
            //        Plot = model.First().Plot,
            //        Poster = model.First().Poster,
            //        Tconst = key,
            //        Genres = model.Select(m => m.Genres).Distinct()
            //        .Skip(page * pageSize).Take(pageSize).ToList()
            //    }
            //    ).Skip(page * pageSize).Take(pageSize).ToList();



            //var temp = new List<DetailedTitleModelDL>();
            //return titles;
            return null;
        }



        /*
         

        public IList<ListTitleModelDL> GetFilteredTitles(List<SearchTitleModel> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();

            //var filteredTitles = _db.FullViewTitles.ToList()
            var filteredTitles = db.FullViewTitles.ToList()
                .Join(searchedTitles,  //inner sequence
                    fullView => fullView.Tconst, //outerKeySelector 
                    searchResults => searchResults.Tconst,     //innerKeySelector
                    (fullView, searchResults)
                                  => fullView
                    )
                ;



            var groupedTitles = filteredTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new ListTitleModelDL
                {

                    BasicTitle = new BasicTitleModelDL
                    {
                        Tconst = model.First().Tconst,
                        PrimaryTitle = model.First().PrimaryTitle,
                        StartYear = model.First().StartYear,
                        TitleType = model.First().TitleType,
                    },
                    Runtime = model.First().Runtime,
                    Rating = model.First().Rating,
                    Genres = model.Select(m => m.Genres).Distinct().ToList(),
                    //ParentTitle = GetBasicTitle(model.FirstOrDefault().ParentTconst)
                    ParentTitle = string.IsNullOrEmpty(model.FirstOrDefault().ParentTconst) ? null : GetBasicTitle(model.FirstOrDefault().ParentTconst)

                })




                .Skip(page * pageSize).Take(pageSize)
                .ToList();

            var listTitles2 = groupedTitles
                   //.GroupJoin(_db.TitleEpisodes,  //inner sequence
                   .GroupJoin(db.TitleEpisodes,  //inner sequence
                       std => std.BasicTitle.Tconst, //outerKeySelector 
                       s => s.Tconst,     //innerKeySelector
                       (std, s) =>
                       //std
                       new ListTitleModelDL
                       {

                           BasicTitle = new BasicTitleModelDL
                           {
                               Tconst = std.BasicTitle.Tconst,
                               PrimaryTitle = std.BasicTitle.PrimaryTitle,
                               StartYear = std.BasicTitle.StartYear,
                               TitleType = std.BasicTitle.TitleType,
                               //TitleType = x.TitleType,
                           },
                           Runtime = std.Runtime,
                           Rating = std.Rating,
                           Genres = std.Genres,
                       }
                       );




            //var listTitles3 = listTitles2.ToList().Select(
            //        x => x.ParentTitle = x.
            //    );


            return groupedTitles;
        }

         */





        public IList<ListTitleModelDL> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5)
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
                .GroupBy(t => t.Tconst, (key, model) => new ListTitleModelDL
                {
                    BasicTitle = new BasicTitleModelDL
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