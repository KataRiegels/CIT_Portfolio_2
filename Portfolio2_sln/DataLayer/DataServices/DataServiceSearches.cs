using System;
using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;

namespace DataLayer.DataServices
{
    public class DataServiceSearches : IDataServiceSearches
    {
        private static ImdbContext _db = new ImdbContext();

        public SearchResult GetSearchResult(int searchId)
        {
            using var db = new ImdbContext();
            var basicTitle = db.UserSearches
                .FirstOrDefault(x => x.SearchId == searchId);

            var result = GenerateSearchResults(basicTitle.SearchContent, basicTitle.SearchCategory);
            result.SearchId = searchId;
            var basic = new SearchResult
            {
                SearchId = searchId,

            };

            return result;

        }


        public SearchResult GenerateSearchResults(string searchContent, string searchCategory = null)
        {
            Console.WriteLine(searchContent);
            using var db = new ImdbContext();

            var searchResult = new SearchResult();

            if (searchCategory != "titles")
            {

            var names  = searchCategory != "titles" ? db.SearchNameResults
                    .FromSqlInterpolated($"select * from string_search_names({searchContent})")
                    //.ToList() : null;
                    .ToList() : null;
            var listNames = GetNamesForSearch(names);

            searchResult.NameResults = listNames;
            }


            if (searchCategory != "names")

            {
                var titles = searchCategory != "names" ? db.SearchTitleResults
                    .FromSqlInterpolated($"select * from string_search_titles({searchContent})")
                    .ToList() : null;
            var listTitles = GetTitlesForSearch(titles);
            searchResult.TitleResults = listTitles;

            }



            return searchResult;
        }



        public IList<ListNameModelDL> GetNamesForSearch(List<SearchNameModel> searchedNames, int page = 0, int pageSize = 20)
        {
            using var db = new ImdbContext();
            Console.WriteLine("before join");



            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var filtered = db.NameBasicss.ToList()
                .Join(searchedNames,  
                    fullView => fullView.Nconst, 
                    searchResults => searchResults.Nconst,  
                    (fullView, searchResults)
                                  => fullView
                    )
                ;


            stopwatch.Stop();
            var elapsed_time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("joining name basics with searched list: ms: " + elapsed_time);

            Console.WriteLine("after filtered");

            stopwatch.Start();
            var query =
                filtered.ToList().GroupJoin(_db.NameKnownFors,
                       basics => basics.Nconst,
                       knownFor => knownFor.Nconst,
                       (basics, knownFor) =>
                       new ListNameModelDL
                       {
                           BasicName = new BasicNameModelDL
                           {
                               Nconst = basics.Nconst,
                               PrimaryName = basics.PrimaryName,
                           },
                           KnownForTitleBasics = knownFor.Any() ? 
                                new DataServiceTitles().
                                GetBasicTitle(knownFor.FirstOrDefault().Tconst) : null
                       }
                           )
                .Skip(page * pageSize).Take(pageSize)
                .ToList();

            //var stopwatch = new Stopwatch();
            stopwatch.Stop();
            elapsed_time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine(elapsed_time);

            Console.WriteLine("after join");

            return query;
        }

        public IList<ListTitleModelDL> GetTitlesForSearch(List<SearchTitleModel> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();

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
                    ParentTitle = string.IsNullOrEmpty(model.FirstOrDefault().ParentTconst) ? null : new DataService().GetBasicTitle(model.FirstOrDefault().ParentTconst)

                })
                .Skip(page * pageSize).Take(pageSize)
                .ToList();

            var listTitles2 = groupedTitles
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
                           },
                           Runtime = std.Runtime,
                           Rating = std.Rating,
                           Genres = std.Genres,
                       }
                       );



            return groupedTitles;
        }
    }
}
