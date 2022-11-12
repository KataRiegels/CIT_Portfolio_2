using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Model;
using DataLayer.Models.TitleModels;
using WebServer.Models.TitleModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography.X509Certificates;
using WebServer.Models.NameModels;

namespace WebServer.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public TitleController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetTitles))]
        public IActionResult GetTitles(string? titleType = null)
        {
            IEnumerable<TitleModel> titles =
                _dataService.GetTitles(titleType).Select(x => CreateTitleModel(x));


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

        // Get all the title_akas
        [HttpGet("{tconst}/akas", Name = nameof(GetTitleAkasByTitle))]
        public IActionResult GetTitleAkasByTitle(string tconst)
        {
            IEnumerable<TitleAka> titles =
                _dataService.GetTitleAkasByTitle(tconst);

            return Ok(titles);

        }

        [HttpGet("basics", Name = nameof(GetBasicTitles))]
        public IActionResult GetBasicTitles()
        {

            IEnumerable<BasicTitleModel> titles =
                _dataService.GetBasicTitles().Select(x => CreateBasicTitleModel(x));


            return Ok(titles);
        }

        [HttpGet("list")]
        public IActionResult GetListTitles()
        {   
            IEnumerable<ListTitleModel> titles =
                _dataService.GetListTitles().Select(x => CreateListTitleModel(x));

            if(titles == null)
            {
                return NotFound();
            }

            return Ok(titles);
        }


        // Get all titles that includes given genre
        //[HttpGet("genre/{genreName}", Name = nameof(GetTitlesByGenre))]
        //public IActionResult GetTitlesByGenre(string genreName)
        //{
        //    IEnumerable<TitleModel> title = _dataService.GetTitlesByGenre(genreName).Select(x => CreateTitleModel(x));
        //    //var title = _dataService.GetTitlesByGenre(genreName); // If we want to return normal TitleBasics instead

        //    if (title == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(title);
        //}

        // Get all titles that includes given genre
        [HttpGet("{tconst}/episodes", Name = nameof(GetEpisodesFromTitle))]
        public IActionResult GetEpisodesFromTitle(string tconst)
        {
            IEnumerable<TitleModel> title = _dataService.GetEpisodesFromTitle(tconst).Select(x => CreateTitleModel(x));
            //var title = _dataService.GetTitlesByGenre(genreName); // If we want to return normal TitleBasics instead

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }

        [HttpGet("PrincipalTitles/{nconst}")]
        public IActionResult GetTitlesPrincipalFromName(string nconst)
        {
            IEnumerable<TitlePrincipalModel> titles =
                _dataService.GetTitlesPrincipalFromName(nconst).Select(x => CreateTitlePrincipalModel(x));

            if (titles == null)
            {
                return NotFound();
            }
            return Ok(titles);
        }

        [HttpGet("Omdbdata/{tconst}")]
        public IActionResult GetOmdbData(string tconst)
        {
            OmdbModel omdb = CreateOmdbModel(_dataService.GetOmdbData(tconst));

            if(omdb == null)
            {
                return NotFound();
            }
            return Ok(omdb);
        }

        [HttpGet("Omdbdata/plot/{tconst}")]
        public IActionResult GetPlot(string tconst)
        {
            string plot = _dataService.GetPlot(tconst);
            return Ok(plot);
        }

        [HttpGet("detailed")]
        public IActionResult GetDetailedTitles()
        {

            //IEnumerable<DetailedTitleModel>? titles =

            //_dataService.GetDetailedTitles().Select(x => CreateDetailedTitleModel(x));
            var titles2 =
            _dataService.GetDetailedTitles();
            Console.WriteLine("skdjflksjdflskflksjdflksjfdlskjflskjdf");
               //var titles = titles2.Select(x => CreateDetailedTitleModel(x));
               var titles = titles2.Select(x => x);

            if (titles == null)
            {
                return NotFound();
            }

            return Ok(titles);
        }

        /* -----------
            HELPERS
         ------------- */

        // Take TitleBasics from Datalayer and makes it into TitleModel to display for client
        public TitleModel CreateTitleModel(TitleBasics titleBasics)
        {
            var model = _mapper.Map<TitleModel>(titleBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);
            
            return model;
        }

        public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleModelDL? detailedTitle)
        {
            Console.WriteLine("I hate my life");
            var model = _mapper.Map<DetailedTitleModel>(detailedTitle);
            //model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

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
            var model = _mapper.Map<ListTitleModel>(titleBasics);
            //model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

            return model;
        }

        //public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleModelDL titleBasics)
        //{
        //    var model = _mapper.Map<DetailedTitleModel>(titleBasics);
        //    model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

        //    return model;
        //}
















        public OmdbModel CreateOmdbModel(OmdbData omdb)
        {
            var model = _mapper.Map<OmdbModel>(omdb);
            return model;
        }

        public TitlePrincipalModel CreateTitlePrincipalModel(TitlePrincipal titleprincipal)
        {
            var model = _mapper.Map<TitlePrincipalModel>(titleprincipal);
            //model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleprincipal.Tconst });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

            return model;
        }
    }
}
