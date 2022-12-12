using DataLayer.DataTransferObjects;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DTOs.TitleObjects;
using Microsoft.EntityFrameworkCore;
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
        public SearchResultDTO GenerateSearchResults(string searchContent, string searchCategory = null)
        {
            using var db = new ImdbContext();

            var searchResult = new SearchResultDTO();

            // Looks for matching names only if client has no specified category as "title" 
            if (searchCategory != "titles")
            {
                var returnedMathingNames  = searchCategory != "titles" ? db.SearchNameResults
                        .FromSqlInterpolated($"select * from string_search_names({searchContent})")
                        .ToList() : null;
                var matchingNames = 
                    new DataServiceNames().GetFilteredNames(
                    returnedMathingNames
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
