using AutoMapper;
using DataLayer;
using DataLayer.DomainModels;
using DataLayer.DomainModels.TitleModels;
using WebServer.Models.TitleModels;
using WebServer.Controllers;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;
using DataLayer.DataServices;


using DataLayer.DomainModels.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography.X509Certificates;
using DataLayer.DomainModels.NameModels;
using WebServer.Models.NameModels;
using NpgsqlTypes;
using AutoMapper;
using DataLayer;
using DataLayer.DomainModels;
using DataLayer.DomainModels.TitleModels;
using DataLayer.DomainModels.UserModels;
using WebServer.Models.TitleModels;
using WebServer.Models.UserModels;
using WebServer.Controllers;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.DomainModels.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography.X509Certificates;
using DataLayer.DTOs.NameObjects;
using System.Xml.Schema;
using DataLayer.DTOs.TitleObjects;

namespace WebServer.Controllers
{
    [Route("api/names")]
    [ApiController]

    public class NameController : ControllerBase
    {
        private IDataServiceNames _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private const int MaxPageSize = 25;

        public NameController(IDataServiceNames dataService, LinkGenerator generator, IMapper mapper)
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

        [HttpGet("{nconst}/contributing", Name = nameof(GetTitleRelations))]
        public IActionResult GetTitleRelations(string nconst, int page = 1, int pageSize = 20)
        {

            var (totalItems, relations) = _dataService
                .GetRelatedTitles(nconst, page, pageSize);

            //Console.WriteLine(relations.First().Nconst);
            Console.WriteLine(relations.Count());

            var result = relations
                .Select(x => MapToCrewModel(x))

                ;

            if (relations == null)
            {
                return NotFound();
            }

            var paging = Paging(page, pageSize, totalItems, result, nameof(GetTitleRelations), nconst);

            return Ok(paging);
        }

        private CrewModel MapToCrewModel(TitleCrewDTO crewDTO)
        {
            var model = new CrewModel().ConvertFromDTO(crewDTO);
            Console.WriteLine(crewDTO.Nconst);
            model.BasicName.Url = CreateNameUrl(crewDTO.Nconst);
            model.BasicTitle.Url = CreateTitleUrl(crewDTO.Tconst);
            return model;
        }

        private NameTitleRelationModel MapNameTitleRelation(NameTitleRelationDTO nameTitleDTO)
        {
            var model = new NameTitleRelationModel().ConvertFromDTO(nameTitleDTO);
            model.Title.Url = CreateTitleUrl(nameTitleDTO.Title.Tconst);
            if (nameTitleDTO.Title.Tconst != null)
            {
                model.Title.Url = CreateTitleUrl(nameTitleDTO.Title.Tconst);
            }

            return model;
        }


        [HttpGet("list", Name = nameof(GetListNames))]
        public IActionResult GetListNames(int page = 1, int pageSize = 20)
        {

            var names =
              _dataService.GetListNames(page, pageSize)
              .Select(x => CreateListNameModel(x));
            //_dataService.GetListNames();

            var total = _dataService.GetNumberOfPeople();

            if(names == null)
            {
                return NotFound();
            }

            return Ok(Paging(page, pageSize, total, names, nameof(GetListNames)));
        }

        [HttpGet("list/{nconst}", Name = nameof(GetListName))]
        public IActionResult GetListName(string nconst)
        {
            var title =
                CreateListNameModel(_dataService.GetListName(nconst));


            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }


        [HttpGet("detailed")]
        public IActionResult GetDetailedNames()
        {
            IEnumerable<DetailedNameModel> names =
                _dataService.GetDetailedNames().Select(x => CreateDetailedNameModel(x));

            if(names == null)
            {
                return NotFound();
            }

            return Ok(names);
        }


        [HttpGet("detailed/{nconst}", Name = nameof(GetDetailedName))]
        public IActionResult GetDetailedName(string nconst)
        {

            var detailedTitle =
            CreateDetailedNameModel(_dataService.GetDetailedName(nconst));

            if (detailedTitle == null)
            {
                return NotFound();
            }

            return Ok(detailedTitle);
        }



        private DetailedNameModel CreateDetailedNameModel(DetailedNameDTO detailModel)
        {
            var model = new DetailedNameModel().ConvertFromDTO(detailModel);
            model.Url = CreateNameUrl(detailModel.Nconst);

            //var model = _mapper.Map<DetailedNameModel>(detailModel);

            return model;
        
        
        }


        private NameForListModel CreateListNameModel(NameForListDTO listModel)
        {
            var model = new NameForListModel().ConvertFromDTO(listModel);
            model.BasicName.Url = _generator.GetUriByName(
                HttpContext, 
                nameof(GetName), 
                new { listModel.BasicName.Nconst });
            //CreateTitleUrl(listModel.BasicName.Nconst);
            model.Url = _generator.GetUriByName(
                HttpContext, 
                nameof(GetListName), 
                new { listModel.BasicName.Nconst });
            //model.BasicTitle.Url = CreateTitleUrl(title.BasicTitle.Tconst);

            if (model.KnownForTitleBasics!= null)
            {
                model.KnownForTitleBasics.Url =
                    _generator.GetUriByName(
                    HttpContext,
                    nameof(TitleController.GetTitle),
                    new { listModel.KnownForTitleBasics.Tconst }); 
                //model.ParentTitle.Url = CreateTitleUrl(title.ParentTitle.Tconst);
            }

            return model;
        }

        private NameModel CreateNameModel(NameBasics nameBasics)
        {
            var model = _mapper.Map<NameModel>(nameBasics);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetName), new { nameBasics.Nconst });

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

        private string? CreateLinkList(int page, int pageSize, string method, string nconst="")
        {
            var uri = _generator.GetUriByName(
                HttpContext,
                method,
                new { nconst, page, pageSize });
            return uri;
        }




        private object Paging<T>(int page, int pageSize, int totalItems, IEnumerable<T> items, string method, string nconst="")
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var totalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize) - 1
                ;

            var firstPageUrl = totalItems > 0
                ? CreateLinkList(1, pageSize, method, nconst)
                : null;

            var prevPageUrl = page > 1 && totalItems > 0
                ? CreateLinkList(page - 1, pageSize, method, nconst)
                : null;

            var lastPageUrl = totalItems > 0
                ? CreateLinkList(totalPages, pageSize, method, nconst)
                : null;

            var currentPageUrl = CreateLinkList(page, pageSize, method, nconst);

            var nextPageUrl = page < totalPages  && totalItems > 0
                ? CreateLinkList(page + 1, pageSize, method, nconst)
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