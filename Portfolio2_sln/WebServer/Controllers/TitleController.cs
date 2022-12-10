using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.DataTransferObjects;
using DataLayer.DataServices;
using DataLayer.Models.TitleModels;
using WebServer.Models.TitleModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using WebServer.Models.NameModels;
using NpgsqlTypes;

namespace WebServer.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private IDataServiceTitles _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public TitleController(IDataServiceTitles dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetTitles))]
        public IActionResult GetTitles()
        {
            IEnumerable<TitleModel> titles =
                _dataService.GetTitles().Select(x => CreateTitleModel(x));


            return Ok(titles);
        }

        [HttpGet("{tconst}", Name = nameof(GetTitle))]
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


            return Ok(titles);
        }


        [HttpGet("list")]
        public IActionResult GetListTitles(int page = 0, int pageSize = 20)
        {
            Console.WriteLine(page);
            //IEnumerable<ListTitleModel> titles =
            IEnumerable<ListTitleModel> titles =
                _dataService.GetListTitles(page, pageSize)
                .Select(x => CreateListTitleModel(x));


            if (titles == null)
            {
                return NotFound();
            }

            return Ok(titles);
        }


        [HttpGet("detailed")]
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
                .Select(x => MapToCastModel(x));

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

        public TitleModel CreateTitleModel(TitleBasics titleBasics)
        {
            var model = _mapper.Map<TitleModel>(titleBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);
            
            return model;
        }

        public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleModelDL? detailedTitle)
        {
            //var model = _mapper.Map<DetailedTitleModel>(detailedTitle);
            var model = new DetailedTitleModel().ConvertFromDetailedTitleDTO(detailedTitle);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { detailedTitle.Tconst });

            return model;
        }

        public BasicTitleModel CreateBasicTitleModel(BasicTitleModelDL titleBasics)
        {

            var model = _mapper.Map<BasicTitleModel>(titleBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

            return model;
        }

        public ListTitleModel CreateListTitleModel(ListTitleModelDL titleBasics)
        {

            //var model1 = _mapper.Map<BasicTitleModel>(titleBasics.BasicTitle);
            var model = _mapper.Map<ListTitleModel>(titleBasics);
            model.BasicTitle = CreateBasicTitleModel(titleBasics.BasicTitle);
            model.BasicTitle.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.BasicTitle.Tconst });
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


        //public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleModelDL titleBasics)
        //{
        //    var model = _mapper.Map<DetailedTitleModel>(titleBasics);
        //    model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

        //    return model;
        //}















    }
}
     


