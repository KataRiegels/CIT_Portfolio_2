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
            Console.WriteLine(basicTitle.SearchContent);

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
            var titles = db.SearchTitleResults.FromSqlInterpolated($"select * from string_search_titles({searchContent})").ToList();
            var names  = db.SearchNameResults.FromSqlInterpolated($"select * from string_search_names({searchContent})").ToList();

            var listTitles = GetTitlesForSearch(titles);
            var listNames = GetNamesForSearch(names);

            searchResult.TitleResults = listTitles;
            searchResult.NameResults = listNames;


            return searchResult;
        }



        public IList<ListNameModelDL> GetNamesForSearch(List<SearchNameModel> searchedNames, int page = 0, int pageSize = 20)
        {
            using var db = new ImdbContext();
            Console.WriteLine("before join");
            /*
            var filteredTitles = db.FullViewNames.ToList()
                .Join(searchedNames,  //inner sequence
                    fullView => fullView.Nconst, //outerKeySelector 
                    searchResults => searchResults.Nconst,     //innerKeySelector
                    (fullView, searchResults)
                                  => fullView
                    )
                ;

            Console.WriteLine("after join");
            Console.WriteLine(filteredTitles.Count());

            var groupedTitles = filteredTitles

                .ToList()
                .GroupBy(t => t.Nconst, (key, model) => new ListNameModelDL
                {
                    BasicName = new BasicNameModelDL
                    {
                        Nconst = key,
                        PrimaryName = model.First().PrimaryName,
                    },
                    //KnownForTitleBasics = model.Select(m => m.KnownForTitle).Distinct().ToList(),
                    KnownForTitleBasics = model.First().KnwonForTconst.Any() ? new DataServiceTitles().GetBasicTitle(model.First().KnwonForTconst) : null
                }
                ).Skip(page * pageSize).Take(pageSize).ToList();

            return groupedTitles;
            */

            var filtered = db.NameBasicss.ToList()
                .Join(searchedNames,  //inner sequence
                    fullView => fullView.Nconst, //outerKeySelector 
                    searchResults => searchResults.Nconst,     //innerKeySelector
                    (fullView, searchResults)
                                  => fullView
                    )
                ;

            var query =
                filtered.ToList().Distinct().GroupJoin(_db.NameKnownFors,
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
                           //KnownForTitleBasics = model.Select(m => m.KnownForTitle).Distinct().ToList(),
                           //KnownForTitleBasics = new DataServiceTitles().GetBasicTitle(knownFor.FirstOrDefault().Tconst),
                           KnownForTitleBasics = knownFor.Any() ? 
                                new DataServiceTitles().
                                GetBasicTitle(knownFor.FirstOrDefault().Tconst) : null
                       }
                           ).ToList();

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
