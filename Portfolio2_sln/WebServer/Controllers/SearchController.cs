using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;
using DataLayer;
using WebServer.Models.TitleModels;
using WebServer.Models.NameModels;
using WebServer.Models.SearchModels;
using WebServer.Models.UserModels;
using DataLayer.DomainModels.TitleModels;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DTOs.SearchObjects;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;

namespace WebServer.Controllers
{
    [Route("api/search")]
        [ApiController]
        public class SearchController : ControllerBase
        {
            private IDataServiceSearches _dataService;
            private readonly LinkGenerator _generator;
            private const int MaxPageSize = 25;


        public SearchController(IDataServiceSearches dataService, LinkGenerator generator)
            {
                _dataService = dataService;
                _generator = generator;


            }

        [HttpGet("{searchId}", Name = nameof(GetSearchResultFromId))]
        public IActionResult GetSearchResultFromId(int searchId)
        {
            //var title = _dataService.GetTitle(tconst);
            var title = _dataService.GetSearchResult(searchId);
            //var generatedResult = _dataService.GenerateSearchResults(title.SearchContent, searchCategory);
            //SearchResultModel results = CreateSearchModel(generatedResult);
            var result = CreateSearchModel(title);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("all", Name = nameof(GetSearchResultNoCategory))]
        public IActionResult GetSearchResultNoCategory(string searchContent)
        {

            (var nameResultsDTO,  var totalNameResults)  = _dataService.GeneratePersonSearchResult(searchContent, 0, 5);
            (var titleResultsDTO, var totalTitleResults) = _dataService.GenerateTitleSearchResult(searchContent, 0, 5);

            var nameResultsUrl  = _generator.GetUriByName(HttpContext, nameof(GetSearchResultNames),  new { searchContent = searchContent });
            var titleResultsUrl = _generator.GetUriByName(HttpContext, nameof(GetSearchResultTitles), new { searchContent = searchContent });

            var nameResults  = nameResultsDTO.Select(x => MapNameSearchResults(x)).ToList();
            var titleResults = titleResultsDTO.Select(x => MapTitleSearchResults(x)).ToList();


            var results = new 
                { TitleResults = new { Url = titleResultsUrl, titleResultItems = titleResults }, 
                  NameResults = new { Url = nameResultsUrl, nameResultItems = nameResults }
        };

            //var title = _dataService.GetTitle(tconst);
            //var generatedResult = _dataService.GenerateSearchResults(searchContent);
            //SearchResultModel results = CreateSearchModel(generatedResult);
            //var results = _dataService.CreateUserSearch(username, searchContent, searchCategory);
            //var titleResults = results.TitleResults;
            //results.Url = _generator.GetUriByName(HttpContext, nameof(GetSearchResultNoCategory), new { searchContent, searchCategory });
            //var test = CreateUserSearchResultsModel(results);

            //if (results == null)
            //{
            //    return NotFound();
            //}

            return Ok(results);
        }

        [HttpGet("names",Name = nameof(GetSearchResultNames))]
        public IActionResult GetSearchResultNames(string searchContent, int page = 1, int pageSize = 10)
        {


            (var nameResults, var totalItems) = _dataService.GeneratePersonSearchResult(searchContent, page, pageSize);
            var model =  nameResults.Select(x => MapNameSearchResults(x))
             .ToList();

            if (model == null)
            {
                return NotFound();
            }

            return Ok(Paging(page, pageSize, totalItems, model, nameof(GetSearchResultNames), searchContent));
        }

        [HttpGet("titles", Name = nameof(GetSearchResultTitles))]
        public IActionResult GetSearchResultTitles(string searchContent, int page = 1, int pageSize = 2)
        {

            (var titleResults, var totalItems) = _dataService.GenerateTitleSearchResult(searchContent, page, pageSize);
            var model = titleResults.Select(x => MapTitleSearchResults(x))
             .ToList();

            if (model == null)
            {
                return NotFound();
            }

            return Ok(Paging(page, pageSize, totalItems, model, nameof(GetSearchResultTitles), searchContent));
        }



        // Map tite list form DTO to WebServer model, including adding URL's
        public TitleForListModel MapTitleSearchResults(TitleForListDTO titleBasics)
        {
            var model = new TitleForListModel().ConvertFromDTO(titleBasics);
            model.BasicTitle.Url = CreateTitleUrl(titleBasics.BasicTitle.Tconst);
            if (titleBasics.ParentTitle != null)
            {
                model.ParentTitle.Url = CreateTitleUrl(titleBasics.ParentTitle.Tconst);
            }

            return model;
        }

        public NameForListModel MapNameSearchResults(NameForListDTO nameResults)
        {

            var model = new NameForListModel().ConvertFromDTO(nameResults);
            model.BasicName.Url = CreateTitleUrl(nameResults.BasicName.Nconst);
            
            if (nameResults.KnownForTitleBasics != null)
            {
                model.KnownForTitleBasics.Url = CreateTitleUrl(nameResults.KnownForTitleBasics.Tconst);
            }
            return model;
        }




        private string CreateTitleUrl(string tconst)
        {
            if (string.IsNullOrEmpty(tconst)) return null;
            return _generator.GetUriByName(HttpContext, nameof(TitleController.GetTitle), new { tconst });
        }


        private string CreateNameUrl(string nconst)
        {
            if (string.IsNullOrEmpty(nconst)) return null;
            return _generator.GetUriByName(HttpContext, nameof(NameController.GetName), new { nconst });
        }

        private string? CreateLink(int page, int pageSize, string method, string searchContent = "")
        {
            return _generator.GetUriByName(
                HttpContext,
                method, 
                new { page, pageSize, searchContent});
        }




        private object Paging<T>(int page, int pageSize, int totalItems, IEnumerable<T> items, string method, string searchContent)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var totalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize) - 1
                ;

            var firstPageUrl = totalItems > 0
                ? CreateLink(1, pageSize, method, searchContent)
                : null;

            var prevPageUrl = page > 1 && totalItems > 0
                ? CreateLink(page - 1, pageSize, method, searchContent)
                : CreateLink(totalPages, pageSize, method, searchContent);

            var lastPageUrl = totalItems > 0
                ? CreateLink(totalPages, pageSize, method, searchContent)
                : null;

            var currentPageUrl = CreateLink(page, pageSize, method, searchContent);

            var nextPageUrl = page < totalPages && totalItems > 0
                ? CreateLink(page + 1, pageSize, method, searchContent)
                : CreateLink(1, pageSize, method, searchContent);

            var result = new
            {
                firstPageUrl,
                prevPageUrl,
                nextPageUrl,
                lastPageUrl,
                currentPageUrl,
                totalItems,
                totalPages,
                items
            };
            return result;
        }


        /*  DELETABLE   */
        public SearchResultModel CreateSearchModel(SearchResultDTO searchResult)
        {
            //var model = _mapper.Map<UserSearchResultsModel>(searchResult);
            var model = new SearchResultModel().ConvertFromDTO(searchResult);
            if (searchResult.TitleResults != null)
            {
                var titleResults = searchResult.TitleResults
                    .Select(x => MapTitleSearchResults(x))
                    .ToList();
                model.TitleResults = titleResults;
            }
            if (searchResult.NameResults != null)
            {
                Console.WriteLine(searchResult.NameResults.Count());
                var nameResults = searchResult.NameResults
                 .Select(x => MapNameSearchResults(x))
                 .ToList();
                model.NameResults = nameResults;
            }
            //var nameResults = searchResult.NameResults;

            return model;
            //model.BasicTitle.Url = CreateTitleUrl(titleBasics.BasicTitle.Tconst);
            //if (titleBasics.ParentTitle != null)
            //{
            //    model.ParentTitle.Url = CreateTitleUrl(titleBasics.ParentTitle.Tconst);
            //}



            //var titleResults = searchResult.TitleResults
            //    .Select(x => MapTitleSearchResults(x))
            //    .ToList();
            //model.TitleResults = titleResults;

            return model;
        }

    }
}
