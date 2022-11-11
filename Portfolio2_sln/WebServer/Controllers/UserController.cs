using AutoMapper;
using DataLayer;
using DataLayer.Models;
using DataLayer.Model;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using WebServer.Models.TitleModels;
using WebServer.Models.UserModels;
using WebServer.Controllers;
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
        //private IDataServiceUser _dataServiceUser;
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


        /* Title bookmarks */

        [HttpGet("{username}/titlebookmark/{tconst}", Name = nameof(GetBookmarkTitle))]
        public IActionResult GetBookmarkTitle(string username, string tconst)
        {
            //var name = _dataService.GetTitle(tconst);
            BookmarkTitleModel name = CreateBookmarkTitleModel(_dataService.GetBookmarkTitle(username,tconst));

            if (name == null)
            {
                return NotFound();
            }
            return Ok(name);
        }

        [HttpPost("{username}/titlebookmarks", Name = nameof(CreateBookmarkTitle))]
        public IActionResult CreateBookmarkTitle(string username, BookmarkTitleCreateModel newBookmark)
        {
            //Console.WriteLine(newBookmark.ToString());
            Console.WriteLine(username);
            var bookmark = _mapper.Map<BookmarkTitle>(newBookmark);
            var returned = _dataService.CreateBookmarkTitle(username, bookmark.Tconst, bookmark.Annotation);
            return CreatedAtRoute(null, CreateBookmarkTitleModel(returned));
        }


        [HttpDelete("{username}/titlebookmarks", Name = nameof(DeleteBookmarkTitle))]
        public IActionResult DeleteBookmarkTitle(string username, BookmarkTitleCreateModel deleteBookmark)
        {
            //Console.WriteLine(newBookmark.ToString());
            Console.WriteLine(username);
            var bookmark = _mapper.Map<BookmarkTitle>(deleteBookmark);
            var returned = _dataService.DeleteBookmarkTitle(username, bookmark.Tconst);
            //return CreatedAtRoute(null, CreateBookmarkTitleModel(returned));
            return CreatedAtRoute(null, returned);
        }

        [HttpGet("{username}/titlebookmarks")]
        public IActionResult GetBookmarksTitleaByUser(string username)
        {
            IEnumerable<BookmarkTitleModel> bookmark =
                _dataService.GetBookmarkTitlesByUser(username).Select(x => CreateBookmarkTitleModel(x));

            return Ok(bookmark);
        }


        /*  Bookmark names */
        /*
         
        [HttpGet("{username}/namebookmark/{tconst}", Name = nameof(GetBookmarkName))]
        public IActionResult GetBookmarkName(string username, string tconst)
        {
            //var name = _dataService.GetName(tconst);
            BookmarkNameModel name = CreateBookmarkNameModel(_dataService.GetBookmarkName(username, tconst));

            if (name == null)
            {
                return NotFound();
            }
            return Ok(name);
        }

        [HttpPost("{username}/namebookmarks", Name = nameof(CreateBookmarkName))]
        public IActionResult CreateBookmarkName(string username, BookmarkNameCreateModel newBookmark)
        {
            //Console.WriteLine(newBookmark.ToString());
            Console.WriteLine(username);
            var bookmark = _mapper.Map<BookmarkName>(newBookmark);
            var returned = _dataService.CreateBookmarkName(username, bookmark.Nconst, bookmark.Annotation);
            return CreatedAtRoute(null, CreateBookmarkNameModel(returned));
        }


        [HttpDelete("{username}/namebookmarks", Name = nameof(DeleteBookmarkName))]
        public IActionResult DeleteBookmarkName(string username, BookmarkNameCreateModel deleteBookmark)
        {
            //Console.WriteLine(newBookmark.ToString());
            Console.WriteLine(username);
            var bookmark = _mapper.Map<BookmarkName>(deleteBookmark);
            var returned = _dataService.DeleteBookmarkName(username, bookmark.Nconst);
            //return CreatedAtRoute(null, CreateBookmarkNameModel(returned));
            return CreatedAtRoute(null, returned);
        }

        [HttpGet("{username}/namebookmarks")]
        public IActionResult GetBookmarksNamesByUser(string username)
        {
            IEnumerable<BookmarkNameModel> bookmark =
                _dataService.GetBookmarkNamesByUser(username).Select(x => CreateBookmarkNameModel(x));

            return Ok(bookmark);
        }


        public BookmarkNameModel CreateBookmarkNameModel(BookmarkName bookmark)
        {
            var model = _mapper.Map<BookmarkNameModel>(bookmark);
            var title = _dataService.GetBasicName(bookmark.Nconst);
            //var something = new {TitleController }.CreateBasicTitleModel(title);
            model.Name = _mapper.Map<BasicNameModel>(title);
            model.Name.Url = _generator.GetUriByName(HttpContext, nameof(WebServer.Controllers.NameController.GetName), new { title.Tconst });

            //model.TitleUrl = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { bookmark.Username });

            return model;
        }
         */

        /* HELPERS */

        public BookmarkTitleModel CreateBookmarkTitleModel(BookmarkTitle bookmark)
        {
            var model = _mapper.Map<BookmarkTitleModel>(bookmark);
            var title = _dataService.GetBasicTitle(bookmark.Tconst);
            //var something = new {TitleController }.CreateBasicTitleModel(title);
            model.Title = _mapper.Map<BasicTitleModel>(title);
            model.Title.Url = _generator.GetUriByName(HttpContext, nameof(WebServer.Controllers.TitleController.GetTitle), new { title.Tconst});

            //model.TitleUrl = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { bookmark.Username });

            return model;
        }
        public UserModel CreateUserModel(User user)
        {
            var model = _mapper.Map<UserModel>(user);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetUser), new { user.Username });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

            return model;
        }

        /*
         
         
         */


    }
}
