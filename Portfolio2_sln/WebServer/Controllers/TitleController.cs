﻿using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Model;
using DataLayer.DataServices;
using DataLayer.Models.TitleModels;
using WebServer.Models.TitleModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using WebServer.Models.NameModels;

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
            var model = _mapper.Map<DetailedTitleModel>(detailedTitle);

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




        //public DetailedTitleModel CreateDetailedTitleModel(DetailedTitleModelDL titleBasics)
        //{
        //    var model = _mapper.Map<DetailedTitleModel>(titleBasics);
        //    model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.Tconst });

        //    return model;
        //}















    }
}
     


