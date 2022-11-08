﻿using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.TitleModels;
using WebServer.Models.TitleModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography.X509Certificates;

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

        // Get all titles that includes given genre
        [HttpGet("genre/{genreName}", Name = nameof(GetTitlesByGenre))]
        public IActionResult GetTitlesByGenre(string genreName)
        {
            IEnumerable<TitleModel> title = _dataService.GetTitlesByGenre(genreName).Select(x => CreateTitleModel(x));
            //var title = _dataService.GetTitlesByGenre(genreName); // If we want to return normal TitleBasics instead

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }

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

        /* -----------
            HELPERS
         ------------- */

        // Take TitleBasics from Datalayer and makes it into TitleModel to display for client
        private TitleModel CreateTitleModel(TitleBasics titleBasics)
        {
            var model = _mapper.Map<TitleModel>(titleBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);
            
            return model;
        }
    }
}
