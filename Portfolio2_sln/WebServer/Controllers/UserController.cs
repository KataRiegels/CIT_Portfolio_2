using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Model;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using WebServer.Models.TitleModels;
using WebServer.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
//using WebServer.Models;

using DataLayer.Models.TitleModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography.X509Certificates;

namespace WebServer.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IDataService _dataService;
        private IDataServiceUser _dataServiceUser;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public UserController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }


        [HttpGet(Name = nameof(GetUsers))]
        public IActionResult GetUsers()
        {

            IEnumerable<UserModel> users =
                _dataService.GetUsers().Select(x => CreateUserModel(x));

            return Ok(users);
        }

        [HttpGet("{username}", Name = nameof(GetUser))]
        public IActionResult GetUser(string username)
        {
            //var title = _dataService.GetTitle(tconst);
            UserModel title = CreateUserModel(_dataService.GetUser(username));

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }

        [HttpPost]
        public IActionResult CreateUser(UserCreateModel newUser)
        {
            var user = _mapper.Map<User>(newUser);
            _dataService.CreateUser(user.Username, user.Password, user.BirthYear, user.Email);
            return CreatedAtRoute(null, CreateUserModel(user));
        }

        [HttpDelete("{username}")]
        public IActionResult DeleteUser(string username)
        {
            var deleted = _dataService.DeleteUser(username);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }




        public UserModel CreateUserModel(User user)
        {
            var model = _mapper.Map<UserModel>(user);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetUser), new { user.Username });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

            return model;
        }

    }
}
