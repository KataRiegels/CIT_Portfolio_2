using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
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

        // Generates 
        public SearchResult GenerateSearchResults(string searchContent, string searchCategory = null)
        {
            using var db = new ImdbContext();

            var searchResult = new SearchResult();

            // Looks for matching names only if client has no specified category as "title" 
            if (searchCategory != "titles")
            {
                var returnedMathingNames  = searchCategory != "titles" ? db.SearchNameResults
                        .FromSqlInterpolated($"select * from string_search_names({searchContent})")
                        .ToList() : null;
                var matchingNames = GetFilteredNames(returnedMathingNames);
                searchResult.NameResults = matchingNames;
            }

            // Looks for matching titles only if client has no specified category as "names" 
            if (searchCategory != "names")
            {
                var returnedMatchingTitles = searchCategory != "names" ? db.SearchTitleResults
                    .FromSqlInterpolated($"select * from string_search_titles({searchContent})")
                    .ToList() : null;
                var matchingTitles = GetFilteredTitles(returnedMatchingTitles);
                searchResult.TitleResults = matchingTitles;
            }

            return searchResult;
        }


        // Getting filtered list form DTO's for names from based on input list
        public IList<ListNameModelDL> GetFilteredNames(List<SearchNameModel> searchedNames, int page = 0, int pageSize = 20)
        {
            using var db = new ImdbContext();


            Console.WriteLine("before join");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Filters names so only contain those that matched the search
            var filtered = db.NameBasicss.ToList()
                .Join(searchedNames,  
                    fullView => fullView.Nconst, 
                    searchResults => searchResults.Nconst,  
                    (fullView, searchResults)
                                  => fullView
                    );


            stopwatch.Stop();
            var elapsed_time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("joining name basics with searched list: ms: " + elapsed_time);

            Console.WriteLine("after filtered");

            stopwatch.Start();

            // Joins the filtered name_basics with known_for to get list form of matching names
            var searchedTitleResults =
                filtered.ToList().GroupJoin(_db.NameKnownFors,
                       basics => basics.Nconst,
                       knownFor => knownFor.Nconst,
                       (basics, knownFor) => new ListNameModelDL
                           {
                               BasicName =
                               new DataServiceNames().
                                    GetBasicName(basics.Nconst),
                           KnownForTitleBasics = knownFor.Any() ? 
                                    new DataServiceTitles().
                                    GetBasicTitle(knownFor.FirstOrDefault().Tconst) : null
                           }
                    )
                .Skip(page * pageSize).Take(pageSize)
                .ToList();

            stopwatch.Stop();
            elapsed_time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine(elapsed_time);

            Console.WriteLine("after join");

            return searchedTitleResults;
        }

        // Getting filtered list form DTO's from based on input list
        public IList<ListTitleModelDL> GetFilteredTitles(List<SearchTitleModel> searchedTitles, int page = 1, int pageSize = 5)
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
                        Tconst       = model.First().Tconst,
                        PrimaryTitle = model.First().PrimaryTitle,
                        StartYear    = model.First().StartYear,
                        TitleType    = model.First().TitleType,
                    },
                    Runtime     = model.First().Runtime,
                    Rating      = model.First().Rating,
                    Genres      = model.Select(m => m.Genre).Distinct().ToList(),
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
