using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



            var titles = _db.FullViewTitles

                .ToList()
                .GroupBy(t => t.Tconst, (key, model) => new DetailedTitleModelDL
                {
                    PrimaryTitle = model.First().PrimaryTitle,
                    startyear = model.First().StartYear,
                    titletype = model.First().TitleType,
                    runtime = model.First().Runtime,
                    rating = model.First().Rating,
                    plot = model.First().Plot,
                    poster = model.First().Poster,
                    Tconst = key,
                    genre = model.Select(m => m.Genre).Distinct()
                    .Skip(page * pageSize).Take(pageSize).ToList()
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();



            var temp = new List<DetailedTitleModelDL>();
            return titles;
        }




        public ListTitleModelDL GetListTitle(string tconst)
        {
            return null;
        }

        public DetailedTitleModelDL GetDetailedTitle(string tconst)
        {

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
                    Genres = model.Select(m => m.Genre).Distinct().ToList(),
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
                               //TitleType = x.titletype,
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



















    }
}