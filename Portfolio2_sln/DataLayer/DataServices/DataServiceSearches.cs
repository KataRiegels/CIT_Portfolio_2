using DataLayer.DataTransferObjects;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DTOs.TitleObjects;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace DataLayer.DataServices
{
    public class DataServiceSearches : IDataServiceSearches
    {
        private static ImdbContext _db = new ImdbContext();

        public SearchResultDTO GetSearchResult(int searchId)
        {
            using var db = new ImdbContext();
            var basicTitle = db.UserSearches
                .FirstOrDefault(x => x.SearchId == searchId);

            var result = GenerateSearchResults(basicTitle.SearchContent, basicTitle.SearchCategory);
            result.SearchId = searchId;
            var basic = new SearchResultDTO
            {
                SearchId = searchId,
            };

            return result;

        }

        // Generates 

        public (IList<NameForListDTO>,int) GeneratePersonSearchResult(string searchContent, int page = 1, int pageSize = 3)
        {
            using var db = new ImdbContext();

            var searchTerms = searchContent.Split(" ");
            var lastTerm = searchTerms.Last();
            var searchString = "";
            Console.WriteLine();
            foreach (var s in searchTerms)
            {
                searchString = searchString + "'" + s + "'";
                if (lastTerm != s) searchString = searchString + ",";
            }

            var sqlSearchString = $"select * from best_match_name({searchString})";

            // Looks for matching names only if client has no specified category as "title" 
            var returnedMatchingNames = db.SearchNameResults
                    //.FromSqlInterpolated($"select * from string_search_names({searchContent})")
                    .FromSqlRaw(sqlSearchString)
                    .OrderByDescending(x => x.Rank)
                    .ToList() ;
            var matchingNames = GetFilteredNames(
                returnedMatchingNames
                , page, pageSize
                )
                ;
            //searchResult.NameResults = matchingNames;

            return( matchingNames,returnedMatchingNames.Count());
        }

        public (IList<TitleForListDTO>, int) GenerateTitleSearchResult(string searchContent, int page = 1, int pageSize = 3)
        {
            using var db = new ImdbContext();
            
            var searchTerms = searchContent.Split(" ");
            var lastTerm = searchTerms.Last();
            var searchString = "";
            
            foreach (var s in searchTerms)
            {
                searchString = searchString + "'" + s + "'";
                if (lastTerm != s) searchString = searchString + ",";
            }

            var sqlSearchString = $"select * from best_match({searchString})";


            Console.WriteLine(sqlSearchString);
            var returnedMatchingTitles = db.SearchTitleResults
                    .FromSqlRaw(sqlSearchString)
                    .OrderByDescending(x => x.Rank)
                    .ToList();
            var matchingTitles = GetFilteredTitles(
                returnedMatchingTitles
                //.Select(x => new TconstObject { Tconst = x.Tconst }).ToList()
                ,page, pageSize)
                ;

            Console.WriteLine(returnedMatchingTitles.Count());

            //return (matchingTitles, returnedMatchingTitles.Count());
            return (matchingTitles, returnedMatchingTitles.Count());

        }


        public IList<TitleForListDTO> GetFilteredTitles(List<TitleSearchResult> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();


            Console.WriteLine(searchedTitles.First().PrimaryTitle);

            var filteredTitles = searchedTitles
                .Skip(page * pageSize).Take(pageSize)
                .Join(db.FullViewTitles,
                    searchResults => searchResults.Tconst,
                    fullView => fullView.Tconst,
                    (searchResults, fullView)
                                  => fullView
                    )
                ;

            Console.WriteLine(filteredTitles.First().PrimaryTitle);

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
                .ToList();


            return groupedTitles;
        }


        public IList<NameForListDTO> GetFilteredNames(List<NameSearchResult> searchedNames, int page = 0, int pageSize = 20)
        {
            using var db = new ImdbContext();


            Console.WriteLine("before join");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Filters names so only contain those that matched the search
            var filtered = searchedNames
                .Join(db.NameBasicss,
                    searchResults => searchResults.Nconst,
                    fullView => fullView.Nconst,
                    (searchResults, fullView)
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
                       (basics, knownFor) => new NameForListDTO
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




        /* 
         * 
         * 
         * 
         * DELETABLE 
         
         
         
         */
        public SearchResultDTO GenerateSearchResults(string searchContent, string searchCategory = null)
        {
            using var db = new ImdbContext();

            var searchResult = new SearchResultDTO();

            // Looks for matching names only if client has no specified category as "title" 
            if (searchCategory != "titles")
            {
                var returnedMatchingNames  = searchCategory != "titles" ? db.SearchNameResults
                        .FromSqlInterpolated($"select * from string_search_names({searchContent})")
                        .ToList() : null;
                var matchingNames = 
                    new DataServiceNames().GetFilteredNames(
                    returnedMatchingNames
                    .Select(x => new NconstObject { Nconst = x.Nconst }).ToList())
                    ;
                searchResult.NameResults = matchingNames;
            }

            // Looks for matching titles only if client has no specified category as "names" 
            if (searchCategory != "names")
            {
                var returnedMatchingTitles = searchCategory != "names" ? db.SearchTitleResults
                    .FromSqlInterpolated($"select * from string_search_titles({searchContent})")
                    .ToList() : null;
                var matchingTitles = new DataServiceTitles().GetFilteredTitles(
                    returnedMatchingTitles
                    .Select(x => new TconstObject { Tconst= x.Tconst}).ToList())
                    ;
                searchResult.TitleResults = matchingTitles;
            }

            return searchResult;
        }


        
        // Getting filtered list form DTO's for names from based on input list
      
        // Getting filtered list form DTO's from based on input list
        //public IList<TitleForListDTO> GetFilteredTitles(List<TitleSearchResult> searchedTitles, int page = 1, int pageSize = 5)
        public IList<TitleForListDTO> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();

            // Filters the FullViewTitles to only have those returned from the string searchString
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
