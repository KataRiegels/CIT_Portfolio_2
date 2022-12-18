using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.DataServices;
using DataLayer.Models.TitleModels;
using WebServer.Models.TitleModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using WebServer.Models.NameModels;
using NpgsqlTypes;
using DataLayer.DTOs.TitleObjects;
using WebServer.Authentication;
using System.Text;

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


        [HttpGet(Name = nameof(GetTitles))]
        //[BasicAuthentication]
        public IActionResult GetTitles()
        {
            IEnumerable<TitleModel> titles =
                _dataService.GetTitles().Select(x => CreateTitleModel(x));


            return Ok(titles);
        }

        [HttpGet("{tconst}", Name = nameof(GetTitle))]
        //[BasicAuthentication]
        //[AdminAuthentiction]
        public IActionResult GetTitle(string tconst)
        {
            
            //var title = _dataService.GetTitle(tconst);
            TitleModel title = CreateTitleModel(_dataService.GetTitle(tconst));

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }



        [HttpGet("basics", Name = nameof(GetBasicTitles))]
        public IActionResult GetBasicTitles(int page = 0, int pageSize = 20)
        {

            IEnumerable<BasicTitleModel> titles =
                _dataService.GetBasicTitles(page, pageSize).Select(x => CreateBasicTitleModel(x));
            var total = _dataService.GetNumberOfTitles();

            return Ok(Paging(page, pageSize, total, titles, nameof(GetBasicTitles)));
        }


        [HttpGet("list", Name = nameof(GetListTitles))]
        public IActionResult GetListTitles(int page = 0, int pageSize = 20)
        {
            Console.WriteLine(page);
            //IEnumerable<TitleForListModel> titles =
            var titles1 =
                _dataService.GetListTitles(page, pageSize);
            Console.WriteLine(titles1.First().Runtime);
            var titles = titles1
                .Select(x => CreateListTitleModel(x));
            var total = _dataService.GetNumberOfTitles();


            if (titles == null)
            {
                return NotFound();
            }

            return Ok(Paging(page, pageSize, total, titles, nameof(GetListTitles)));
        }


        [HttpGet("detailed")]
        //[BasicAuthentication]
        public IActionResult GetDetailedTitles(int page = 0, int pageSize = 2)
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
            //string user = HttpContext.Request.Headers["Authorization"];
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

        [HttpGet("{tconst}/crew", Name = nameof(GetTitleCrew))]
        public IActionResult GetTitleCrew(string tconst)
        {

            var crew = _dataService
                .GetTitleCrew(tconst)
                .Select(x => MapToCrewModel(x))
                ;

            if (crew == null)
            {
                return NotFound();
            }

            return Ok(crew);
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

            if (tvSeason.Episodes != null)
            {
                var titleResults = tvSeason.Episodes
                    .Select(x => MapTvEpisodeModel(x))
                    .ToList();
                convertedSeason.Episodes = titleResults;
            }
            return convertedSeason;

        }

        private TvSeriesEpisodeModel MapTvEpisodeModel(TvSeriesEpisodeDTO episode)
        {
            var model = new TvSeriesEpisodeModel().ConvertFromDTO(episode);
            model.Url = CreateTitleUrl(episode.Tconst);
            if (episode.Tconst != null)
            {
                model.Url = CreateTitleUrl(episode.Tconst);
            }

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

            //var model = _mapper.Map<TitleModel>(titleBasics);

            var test = nameof(GetTitle);
            var test2 = HttpContext;
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);
            
            return model;
        }

        public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleDTO? detailedTitle)
        {
            //var model = _mapper.Map<DetailedTitleModel>(detailedTitle);
            var model = new DetailedTitleModel().ConvertFromDetailedTitleDTO(detailedTitle);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { detailedTitle.Tconst });

            return model;
        }

        public BasicTitleModel CreateBasicTitleModel(BasicTitleDTO titleBasics)
        {

            //var model = _mapper.Map<BasicTitleModel>(titleBasics);
            var model = new BasicTitleModel().ConvertBasicTitleModel(titleBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

            return model;
        }

        public TitleForListModel CreateListTitleModel(TitleForListDTO titleBasics)
        {

            //var model1 = _mapper.Map<BasicTitleModel>(titleBasics.BasicTitle);
            //var model = _mapper.Map<TitleForListModel>(titleBasics);
            var model = new TitleForListModel().ConvertFromDTO(titleBasics);
            model.BasicTitle.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.BasicTitle.Tconst });
            model.BasicTitle = CreateBasicTitleModel(titleBasics.BasicTitle);
            Console.WriteLine(titleBasics.Rating);
            model.BasicTitle.Url = CreateTitleUrl(titleBasics.BasicTitle.Tconst);

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


        //public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleDTO titleBasics)
        //{
        //    var model = _mapper.Map<DetailedTitleModel>(titleBasics);
        //    model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

        //    return model;
        //}


        private string? CreateLinkList(int page, int pageSize, string method, string searchContent = "")
        {
            
            var uri = _generator.GetUriByName(
                HttpContext,
                method,
                new {  page, pageSize, searchContent });
            return uri;
        }




        private object Paging<T>(int page, int pageSize, int totalItems, IEnumerable<T> items, string method)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var totalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize) - 1
                ;

            var firstPageUrl = totalItems > 0
                ? CreateLinkList(0, pageSize, method)
                : null;

            var prevPageUrl = page > 0 && totalItems > 0
                ? CreateLinkList(page - 1, pageSize, method)
                : null;

                var lastPageUrl = totalItems > 0
                ? CreateLinkList(totalPages, pageSize, method)
                : null;

            var currentPageUrl = CreateLinkList(page, pageSize, method);

            var nextPageUrl = page < totalPages - 1 && totalItems > 0
                ? CreateLinkList(page + 1, pageSize, method)
                : null;

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
     


