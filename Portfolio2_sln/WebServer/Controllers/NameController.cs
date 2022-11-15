using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.TitleModels;
using WebServer.Models.TitleModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography.X509Certificates;
using DataLayer.Models.NameModels;
using WebServer.Models.NameModels;
using DataLayer.Model;
using NpgsqlTypes;

namespace WebServer.Controllers
{
    [Route("api/names")]
    [ApiController]

    public class NameController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public NameController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNames(int page = 0, int pageSize = 20)
        {
            IEnumerable<NameModel> names =
                _dataService.GetNames(page, pageSize).Select(x => CreateNameModel(x));
            return Ok(names);
        }

        [HttpGet("{nconst}", Name = nameof(GetName))]
        public IActionResult GetName(string nconst)
        {
            NameModel name = CreateNameModel(_dataService.GetName(nconst));

            if(name == null)
            {
                return NotFound();
            }

            return Ok(name);
        }

        [HttpGet("list")]
        public IActionResult GetListNames(int page = 0, int pageSize = 20)
        {
            IEnumerable<ListNameModel> names =
              _dataService.GetListNames(page, pageSize).Select(x => CreateListNameModel(x));

            if(names == null)
            {
                return NotFound();
            }

            return Ok(names);
        }

        [HttpGet("detailed")]
        public IActionResult GetDetailedNames(int page = 0, int pageSize = 20)
        {
            IEnumerable<DetailedNameModel> names =
                _dataService.GetDetailedNames(page, pageSize).Select(x => CreateDetailedNameModel(x));

            if(names == null)
            {
                return NotFound();
            }

            return Ok(names);
        }

        private DetailedNameModel CreateDetailedNameModel(DetailedNameModelDL detailModel)
        {
            var model = _mapper.Map<DetailedNameModel>(detailModel);
            return model;
        }


        private ListNameModel CreateListNameModel(ListNameModelDL listModel)
        {
            var model = _mapper.Map<ListNameModel>(listModel);
            return model;
        }

        private NameModel CreateNameModel(NameBasics nameBasics)
        {
            var model = _mapper.Map<NameModel>(nameBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetName), new { nameBasics.Nconst });

            return model;
        }


    }
}