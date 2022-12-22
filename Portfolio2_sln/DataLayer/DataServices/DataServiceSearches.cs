using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DTOs.TitleObjects;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices
{
    public class DataServiceSearches : IDataServiceSearches
    {

        public (IList<NameForListDTO>, int) GeneratePersonSearchResult(string searchContent, int page = 1, int pageSize = 3)
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

            var sqlSearchString = $"select * from best_match_name({searchString})";

            // Looks for matching names only if client has no specified category as "title" 
            var returnedMatchingNames = db.SearchNameResults
                    .FromSqlRaw(sqlSearchString)
                    .OrderByDescending(x => x.Rank)
                    .ToList();
            var matchingNames = GetFilteredNames(
                returnedMatchingNames
                , page, pageSize
                );

            return (matchingNames, returnedMatchingNames.Count());
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
                , page, pageSize)
                ;

            Console.WriteLine(returnedMatchingTitles.Count());

            return (matchingTitles, returnedMatchingTitles.Count());

        }


        public IList<TitleForListDTO> GetFilteredTitles(List<TitleSearchResult> searchedTitles, int page = 1, int pageSize = 5)
        {
            using var db = new ImdbContext();


            Console.WriteLine(searchedTitles.First().PrimaryTitle);

            var filteredTitles = searchedTitles
                .Skip((page - 1) * pageSize).Take(pageSize)
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
                                    : new DataServiceTitles().GetBasicTitle(model.FirstOrDefault().ParentTconst)
                })
                .ToList();


            return groupedTitles;
        }


        public IList<NameForListDTO> GetFilteredNames(List<NameSearchResult> searchedNames, int page = 0, int pageSize = 20)
        {
            using var db = new ImdbContext();


            // Filters names so only contain those that matched the search
            var filtered = searchedNames
                .Join(db.NameBasicss,
                    searchResults => searchResults.Nconst,
                    fullView => fullView.Nconst,
                    (searchResults, fullView)
                                  => fullView
                    );

            // Joins the filtered name_basics with known_for to get list form of matching names
            var searchedTitleResults =
                filtered.ToList().GroupJoin(db.NameKnownFors,
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
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToList();

            return searchedTitleResults;
        }




    }
}
