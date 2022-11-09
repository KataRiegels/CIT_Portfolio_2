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
        public IActionResult GetNames()
        {
            IEnumerable<NameModel> names =
                _dataService.GetNames().Select(x => CreateNameModel(x));
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

        

        private NameModel CreateNameModel(NameBasics nameBasics)
        {
            var model = _mapper.Map<NameModel>(nameBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetName), new { nameBasics.Nconst });

            return model;
        }


    }
}