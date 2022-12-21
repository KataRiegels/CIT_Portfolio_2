using AutoMapper;
using DataLayer;
using DataLayer.DomainModels;
using DataLayer.DataServices;
using DataLayer.DomainModels.TitleModels;
using WebServer.Models.TitleModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.DomainModels.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using WebServer.Models.NameModels;
using NpgsqlTypes;
using DataLayer.DTOs.TitleObjects;
using WebServer.Authentication;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebServer.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private IDataServiceTitles _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private const int MaxPageSize = 25;
        public TitleController(IDataServiceTitles dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet( Name = nameof(GetTitles))]

        //[HttpGet(Name = nameof(GetTitles))]
        //[BasicAuthentication]
        public IActionResult GetTitles(int page = 1, int pageSize = 20)
        {
            IEnumerable<BasicTitleModel> titles =
                _dataService.GetBasicTitles(page, pageSize).Select(x => CreateBasicTitleModel(x));
            var total = _dataService.GetNumberOfTitles();

            return Ok(Paging(page, pageSize, total, titles, nameof(GetTitles)));

        }

        [HttpGet("{tconst}", Name = nameof(GetTitle))]
        //[BasicAuthentication]
        //[AdminAuthentiction]
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


        [HttpGet("detailed")]
        public IActionResult GetDetailedTitles(int page = 1, int pageSize = 2)
        {

            var titles =
            _dataService.GetDetailedTitles(page, pageSize);

            if (titles == null)
            {
                return NotFound();
            }

            return Ok(titles);
        }



        [HttpGet("detailed/{tconst}", Name = nameof(GetDetailedTitle))]
        //[BasicAuthentication]
        //[AdminAuthentiction]
        public IActionResult GetDetailedTitle(string tconst)
        {
            //string quser = HttpContext.Request.Headers["Authorization"];
            //string encodedUsernamePassword = user.Remove(0,"Basic ".Length).Trim();
            //Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            //string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            //Console.WriteLine(usernamePassword);
            var detailedTitle =
            CreateDetailedTitleModel(_dataService.GetDetailedTitle(tconst));

            if (detailedTitle == null)
            {
                return NotFound();
            }

            return Ok(detailedTitle);
        }


        [HttpGet("{tconst}/cast", Name = nameof(GetTitleCast))]
        public IActionResult GetTitleCast(string tconst)
        {

            var cast = _dataService
                .GetTitleCast(tconst)
                .Select(x => MapToCastModel(x));

            if (cast == null)
            {
                return NotFound();
            }

            return Ok(cast);
        }

        // Technically missing the self-reference
        [HttpGet("{tconst}/crew", Name = nameof(GetTitleCrew))]
        public IActionResult GetTitleCrew(string tconst, int page = 1, int pageSize = 50)
        {

            var (totalItems, crewDTO) = _dataService 
                .GetTitleCrew(tconst, page, pageSize)
                ;
            var crew = crewDTO.Select(x => MapToCrewModel(x));

            if (crew == null)
                return NotFound();
            var paging = PagingEpisodes(page, pageSize, totalItems, crew, nameof(GetTitleCrew), tconst);
            return Ok(paging);
        }

        [HttpGet("{tconst}/episodes/{episodeNumber}", Name = nameof(GetTitleSeasonEpisode))]
        public IActionResult GetTitleSeasonEpisode(string tconst,  int episodeNumber, int seasonNumber)
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
            Console.WriteLine(episode);
            if (episode != null)
            {
                Console.WriteLine(episode);
                model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitleSeasonEpisode), new { Tconst = episode.ParentTconst, SeasonNumber = episode.SeasonNumber, EpisodeNumber = episode.EpisodeNumber });
            }
            Console.WriteLine(model.Url);

            return model;
        }


        /* -----------
            HELPERS
         ------------- */

        private CastModel MapToCastModel(TitleCastDTO castDTO)
        {
            var model = new CastModel().ConvertFromDTO(castDTO);
            model.BasicName.Url = CreateNameUrl(castDTO.Nconst);
            return model;
        }


        private CrewModel MapToCrewModel(TitleCrewDTO crewDTO)
        {
            var model = new CrewModel().ConvertFromDTO(crewDTO);
            model.BasicName.Url = CreateNameUrl(crewDTO.Nconst);
            return model;
        }

        public TitleModel CreateTitleModel(TitleBasics titleBasics)
        {
            var model = new TitleModel
            {
                Tconst = titleBasics.Tconst,
            };

            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });
            
            return model;
        }

        public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleDTO? detailedTitle)
        {
            //var model = _mapper.Map<DetailedTitleModel>(detailedTitle);
            var model = new DetailedTitleModel().ConvertFromDetailedTitleDTO(detailedTitle);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetDetailedTitle), new { detailedTitle.Tconst });

            return model;
        }

        public BasicTitleModel CreateBasicTitleModel(BasicTitleDTO titleBasics)
        {

            //var model = _mapper.Map<BasicTitleModel>(titleBasics);
            var model = new BasicTitleModel().ConvertBasicTitleModel(titleBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

            return model;
        }

        public TitleForListModel CreateListTitleModel(TitleForListDTO title)
        {

            var model = new TitleForListModel().ConvertFromDTO(title);
            model.BasicTitle.Url = CreateTitleUrl(title.BasicTitle.Tconst);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetListTitle), new { title.BasicTitle.Tconst });
            //model.BasicTitle.Url = CreateTitleUrl(title.BasicTitle.Tconst);

            if (title.ParentTitle != null)
            {
                model.ParentTitle.Url = CreateTitleUrl(title.ParentTitle.Tconst);
            }

            return model;
            /*
             
            //var model1 = _mapper.Map<BasicTitleModel>(titleBasics.BasicTitle);
            var model = _mapper.Map<TitleForListModel>(titleBasics);
            model.BasicTitle = CreateBasicTitleModel(titleBasics.BasicTitle);
            Console.WriteLine(titleBasics.Rating);
            model.BasicTitle.Url = CreateTitleUrl(titleBasics.BasicTitle.Tconst);

            return model;
             */
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


        //public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleDTO titleBasics)
        //{
        //    var model = _mapper.Map<DetailedTitleModel>(titleBasics);
        //    model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

        //    return model;
        //}


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
            ? CreateLinkList(totalPages , pageSize, method, seasonNumber)
            : null;

            var currentPageUrl = CreateLinkList(page, pageSize, method, seasonNumber);

            var nextPageUrl = page < totalPages  && totalItems > 0
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


        private string? CreateLinkEpisodes(string tconst, int page, int pageSize, string method,  string seasonNumber = "")
        {

            //var uri = _generator.GetUriByName(
            var uri = _generator.GetUriByName(
                HttpContext,
                method,
                new { page, pageSize, tconst, seasonNumber });
            //new {  page, pageSize });
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
            ? CreateLinkEpisodes(parentTconst, totalPages , pageSize, method, seasonNumber)
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


        /*
         
        DELETABLE
         
         
         */


        [HttpGet("basics", Name = nameof(GetBasicTitles))]
        public IActionResult GetBasicTitles(int page = 1, int pageSize = 20)
        {

            IEnumerable<BasicTitleModel> titles =
                _dataService.GetBasicTitles(page, pageSize).Select(x => CreateBasicTitleModel(x));
            var total = _dataService.GetNumberOfTitles();

            return Ok(Paging(page, pageSize, total, titles, nameof(GetBasicTitles)));
        }


        [HttpGet("{tconst}/seasons/{seasonNumber}/episodes", Name = nameof(GetTitleSeasonEpisodesSeason))]
        public IActionResult GetTitleSeasonEpisodesSeason(string tconst, int seasonNumber, int episodeNumber)
        {

            var episodesDTO =
                _dataService.GetTvSeriesSeason(tconst, seasonNumber);
            Console.WriteLine("before mapping: " + episodesDTO.Episodes.First().SeasonNumber);

            var episodes = CreateTvSeasonModel(episodesDTO)
                .Episodes;

            if (episodes == null)
            {
                Console.WriteLine("episodes were null");
                return NotFound();
            }

            return Ok(episodes);
        }


        [HttpGet("{tconst}/seasons/{seasonNumber}", Name = nameof(GetTitleSeason))]
        public IActionResult GetTitleSeason(string tconst, int seasonNumber)
        {

            var season =
            CreateTvSeasonModel(_dataService.GetTvSeriesSeason(tconst, seasonNumber))
            ;

            if (season == null)
            {
                return NotFound();
            }

            return Ok(season);
        }


        private TvSeriesSeasonModel CreateTvSeasonModel(TvSeriesSeasonDTO tvSeason)
        {
            var convertedSeason = new TvSeriesSeasonModel().ConvertFromDTO(tvSeason);

            if (tvSeason.Episodes == null)
            {
                return convertedSeason;
            }


            var titleResults = tvSeason.Episodes
                .Select(x => MapTvEpisodeModel(x))
                .ToList();
            convertedSeason.Episodes = titleResults;
            return convertedSeason;
        }


    }
}
     


