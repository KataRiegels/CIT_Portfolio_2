using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.TitleModels;
using WebServer.Models.TitleModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.Query;

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

        [HttpGet]
        public IActionResult GetTitles()
        {

            var titles =
                _dataService.GetTitles().Select(x => CreateTitleModel(x));
            //Console.WriteLine(titles);
            //Console.WriteLine();
            return Ok(titles);
        }

        [HttpGet("{tconst}", Name = nameof(GetTitle))]
        public IActionResult GetTitle(string tconst)
        {
            var title = _dataService.GetTitle(tconst);
            Console.WriteLine(tconst);
            //var title = CreateTitleModel(_dataService.GetTitle(tconst));

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }


        [HttpGet("genre/{genreName}", Name = nameof(GetTitlesByGenre))]
        public IActionResult GetTitlesByGenre(string genreName)
        {
            var title = _dataService.GetTitlesByGenre(genreName);

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }




        private TitleModel CreateTitleModel(TitleBasics titleBasics)
        {
            var model = _mapper.Map<TitleModel>(titleBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });
            model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);
            
            return model;
        }
    }
}
