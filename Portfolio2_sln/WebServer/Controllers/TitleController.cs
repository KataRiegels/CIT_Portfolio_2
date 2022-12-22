using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DomainModels.TitleModels;
//using WebServer.Models;

using DataLayer.DTOs.TitleObjects;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.TitleModels;

namespace WebServer.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private IDataServiceTitles _dataService;
        private readonly LinkGenerator _generator;
        private const int MaxPageSize = 25;
        public TitleController(IDataServiceTitles dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
        }

        [HttpGet(Name = nameof(GetTitles))]

        public IActionResult GetTitles(int page = 1, int pageSize = 20)
        {
            IEnumerable<BasicTitleModel> titles =
                _dataService.GetBasicTitles(page, pageSize).Select(x => CreateBasicTitleModel(x));
            var total = _dataService.GetNumberOfTitles();

            return Ok(Paging(page, pageSize, total, titles, nameof(GetTitles)));

        }

        [HttpGet("{tconst}", Name = nameof(GetTitle))]
        public IActionResult GetTitle(string tconst)
        {

            //var title = _dataService.GetTitle(tconst);
            var title = CreateBasicTitleModel(_dataService.GetBasicTitle(tconst));

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }





        [HttpGet("list", Name = nameof(GetListTitles))]
        public IActionResult GetListTitles(int page = 1, int pageSize = 20)
        {
            var titles =
                _dataService.GetListTitles(page, pageSize)
                .Select(x => CreateListTitleModel(x));
            var total = _dataService.GetNumberOfTitles();


            if (titles == null)
            {
                return NotFound();
            }

            return Ok(Paging(page, pageSize, total, titles, nameof(GetListTitles)));
        }

        [HttpGet("list/{tconst}", Name = nameof(GetListTitle))]
        public IActionResult GetListTitle(string tconst)
        {
            var title =
                CreateListTitleModel(_dataService.GetListTitle(tconst));


            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }



        [HttpGet("detailed/{tconst}", Name = nameof(GetDetailedTitle))]
        public IActionResult GetDetailedTitle(string tconst)
        {
            var detailedTitle =
            CreateDetailedTitleModel(_dataService.GetDetailedTitle(tconst));

            if (detailedTitle == null)
            {
                return NotFound();
            }

            return Ok(detailedTitle);
        }


        // Technically missing the self-reference
        [HttpGet("{tconst}/crew", Name = nameof(GetTitleCrew))]
        public IActionResult GetTitleCrew(string tconst, int page = 1, int pageSize = 50)
        {

            var (totalItems, crewDTO) = _dataService
                .GetTitleCrew(tconst, page, pageSize);
            var crew = crewDTO.Select(x => MapToCrewModel(x));

            if (crew == null)
                return NotFound();

            var paging = PagingEpisodes(page, pageSize, totalItems, crew, nameof(GetTitleCrew), tconst);
            return Ok(paging);
        }

        [HttpGet("{tconst}/episodes/{episodeNumber}", Name = nameof(GetTitleSeasonEpisode))]
        public IActionResult GetTitleSeasonEpisode(string tconst, int episodeNumber, int seasonNumber)
        {

            var episodeDTO = _dataService.GetTvSeriesEpisode(tconst, seasonNumber, episodeNumber);
            var episode = MapTvEpisodeModel(episodeDTO);

            if (episode == null)
                return NotFound();

            return Ok(episode);
        }

        [HttpGet("{tconst}/episodes", Name = nameof(GetTitleSeasonEpisodes))]
        public IActionResult GetTitleSeasonEpisodes(string tconst, int seasonNumber, int page = 1, int pageSize = 100)
        {

            var (total, episodesDTO) =
                _dataService.GetTvSeriesEpisodes(tconst, seasonNumber, page, pageSize);

            var episodes = episodesDTO
                .Select(e => MapTvEpisodeModel(e));



            if (episodes == null)
            {
                return NotFound();
            }

            var paging = PagingEpisodes(page, pageSize, total, episodes, nameof(GetTitleSeasonEpisodes), tconst, seasonNumber.ToString());



            return Ok(paging);
        }



        private TvSeriesEpisodeModel MapTvEpisodeModel(TvSeriesEpisodeDTO episode)
        {
            var model = new TvSeriesEpisodeModel().ConvertFromDTO(episode);
            if (episode != null)
            {
                model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitleSeasonEpisode), new { Tconst = episode.ParentTconst, SeasonNumber = episode.SeasonNumber, EpisodeNumber = episode.EpisodeNumber });
            }

            return model;
        }


        /* -----------
            HELPERS
         ------------- */


        private CrewModel MapToCrewModel(TitleCrewDTO crewDTO)
        {
            var model = new CrewModel().ConvertFromDTO(crewDTO);
            model.BasicName.Url = CreateNameUrl(crewDTO.Nconst);
            return model;
        }

        private DetailedTitleModel CreateDetailedTitleModel(DetailedTitleDTO? detailedTitle)
        {
            var model = new DetailedTitleModel().ConvertFromDTO(detailedTitle);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetDetailedTitle), new { detailedTitle.Tconst });

            return model;
        }

        private BasicTitleModel CreateBasicTitleModel(BasicTitleDTO titleBasics)
        {

            var model = new BasicTitleModel().ConvertFromDTO(titleBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

            return model;
        }

        private TitleForListModel CreateListTitleModel(TitleForListDTO title)
        {

            var model = new TitleForListModel().ConvertFromDTO(title);
            model.BasicTitle.Url = CreateTitleUrl(title.BasicTitle.Tconst);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetListTitle), new { title.BasicTitle.Tconst });

            if (title.ParentTitle != null)
            {
                model.ParentTitle.Url = CreateTitleUrl(title.ParentTitle.Tconst);
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


        private string? CreateLinkList(int page, int pageSize, string method, string seasonNumber = "")
        {
            var uri = _generator.GetUriByName(
                HttpContext,
                method,
                new { page, pageSize, seasonNumber });
            return uri;
        }


        private object Paging<T>(int page, int pageSize, int totalItems, IEnumerable<T> items, string method, string seasonNumber = "")
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var totalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize)
                ;


            var firstPageUrl = totalItems > 0
                ? CreateLinkList(1, pageSize, method, seasonNumber)
                : null;


            var prevPageUrl = page > 1 && totalItems > 0
                ? CreateLinkList(page - 1, pageSize, method, seasonNumber)
                : CreateLinkList(totalPages, pageSize, method, seasonNumber);

            var lastPageUrl = totalItems > 0
            ? CreateLinkList(totalPages, pageSize, method, seasonNumber)
            : null;

            var currentPageUrl = CreateLinkList(page, pageSize, method, seasonNumber);

            var nextPageUrl = page < totalPages && totalItems > 0
                ? CreateLinkList(page + 1, pageSize, method, seasonNumber)
                : CreateLinkList(1, pageSize, method, seasonNumber);

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


        private string? CreateLinkEpisodes(string tconst, int page, int pageSize, string method, string seasonNumber = "")
        {

            var uri = _generator.GetUriByName(
                HttpContext,
                method,
                new { page, pageSize, tconst, seasonNumber });
            return uri;
        }


        private object PagingEpisodes<T>(int page, int pageSize, int totalItems, IEnumerable<T> items, string method, string parentTconst, string seasonNumber = "")
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var totalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize)
                ;


            var firstPageUrl = totalItems > 0
                ? CreateLinkEpisodes(parentTconst, 1, pageSize, method, seasonNumber)
                : null;


            var prevPageUrl = page > 1 && totalItems > 0
                ? CreateLinkEpisodes(parentTconst, page - 1, pageSize, method, seasonNumber)
                : CreateLinkEpisodes(parentTconst, totalPages, pageSize, method, seasonNumber);

            var lastPageUrl = totalItems > 0
            ? CreateLinkEpisodes(parentTconst, totalPages, pageSize, method, seasonNumber)
            : null;

            var currentPageUrl = CreateLinkEpisodes(parentTconst, page, pageSize, method, seasonNumber);

            var nextPageUrl = page < totalPages && totalItems > 0
                ? CreateLinkEpisodes(parentTconst, page + 1, pageSize, method, seasonNumber)
                : CreateLinkEpisodes(parentTconst, 1, pageSize, method, seasonNumber);

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



    }
}



