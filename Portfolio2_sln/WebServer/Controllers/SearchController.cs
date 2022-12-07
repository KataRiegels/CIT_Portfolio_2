using AutoMapper;
using DataLayer.DataServices;
using Microsoft.AspNetCore.Mvc;
using DataLayer;
using WebServer.Models.TitleModels;
using DataLayer.DataTransferObjects;
using WebServer.Models.NameModels;
using WebServer.Models.SearchModels;
using WebServer.Models.UserModels;
using DataLayer.Models.TitleModels;

namespace WebServer.Controllers
{
        [Route("api/search")]
        [ApiController]
        public class SearchController : ControllerBase
        {
            private IDataServiceSearches _dataService;
            //private IDataService _dataServiceTitle;
            //private IDataServiceUser _dataServiceUser;
            private readonly LinkGenerator _generator;
            private readonly IMapper _mapper;
            private TitleController _titleController;

            public SearchController(IDataServiceSearches dataService, LinkGenerator generator, IMapper mapper)
            {
                _dataService = dataService;
                _generator = generator;
                _mapper = mapper;


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

        [HttpGet( Name = nameof(GetSearchResult))]
        public IActionResult GetSearchResult(string searchContent, string? searchCategory = null)
        {



            //var title = _dataService.GetTitle(tconst);
            var generatedResult = _dataService.GenerateSearchResults(searchContent, searchCategory);
            SearchResultModel results = CreateSearchModel(generatedResult);
            //var results = _dataService.CreateUserSearch(username, searchContent, searchCategory);
            var titleResults = results.TitleResults;
            results.Url = _generator.GetUriByName(HttpContext, nameof(GetSearchResult), new { searchContent, searchCategory });
            //var test = CreateUserSearchResultsModel(results);

            if (results == null)
            {
                return NotFound();
            }

            return Ok(results);
        }

        public SearchResultModel CreateSearchModel(SearchResult searchResult)
        {
            //var model = _mapper.Map<UserSearchResultsModel>(searchResult);
            var model = new SearchResultModel().ConvertFromSearchResultDTO(searchResult);
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

        // Map tite list form DTO to WebServer model, including adding URL's
        public ListTitleModel MapTitleSearchResults(ListTitleModelDL titleBasics)
        {
            var model = new ListTitleModel().ConvertFromListTitleDTO(titleBasics);
            model.BasicTitle.Url = CreateTitleUrl(titleBasics.BasicTitle.Tconst);
            if (titleBasics.ParentTitle != null)
            {
                model.ParentTitle.Url = CreateTitleUrl(titleBasics.ParentTitle.Tconst);
            }

            return model;
        }

        public ListNameModel MapNameSearchResults(ListNameModelDL nameResults)
        {

            var model = new ListNameModel().ConvertFromListTitleDTO(nameResults);
            model.BasicName.Url = CreateTitleUrl(nameResults.BasicName.Nconst);
            if (nameResults.KnownForTitleBasics != null)
            {
                model.KnownForTitleBasics.Url = CreateTitleUrl(nameResults.KnownForTitleBasics.Tconst);
            }
            //var model = _mapper.Map<ListNameModel>(nameResults);
            //model.BasicName = _mapper.Map<BasicNameModel>(model.BasicName);
            //model.BasicName.Url = _generator.GetUriByName(HttpContext, nameof(NameController.GetName), new { nameResults.BasicName.Nconst });
            return model;
        }




        private string CreateTitleUrl(string tconst)
        {
            tconst = tconst.Trim();
            if (string.IsNullOrEmpty(tconst)) return null;
            return _generator.GetUriByName(HttpContext, nameof(TitleController.GetTitle), new { tconst });
        }


        private string CreateNameUrl(string nconst)
        {
            nconst = nconst.Trim();
            if (string.IsNullOrEmpty(nconst)) return null;
            return _generator.GetUriByName(HttpContext, nameof(NameController.GetName), new { nconst });
        }

    }
}
