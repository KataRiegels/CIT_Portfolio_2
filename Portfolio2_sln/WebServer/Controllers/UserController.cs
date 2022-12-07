using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DataTransferObjects;
using DataLayer.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.NameModels;
using WebServer.Models.TitleModels;
using WebServer.Models.UserModels;
//using WebServer.Models;


namespace WebServer.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IDataServiceUser _dataService;
        //private IDataService _dataServiceTitle;
        //private IDataServiceUser _dataServiceUser;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private TitleController _titleController;

        public UserController(IDataServiceUser dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;


            //_titleController = new TitleController(_dataService, _generator, _mapper);
            //_titleController.ControllerContext = ControllerContext;
            //_titleController.HttpContext = HttpContext;
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
            //var dsUser = new DataServiceUser();
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

        //[HttpGet("{username}/titlebookmark/{tconst}", Name = nameof(GetBookmarkTitle))]
        //[HttpGet("bookmark")]
        ////public IActionResult GetBookmarkTitle(string username, string tconst)
        //public IActionResult GetBookmarkTitle()
        //{
        //    //BookmarkTitleModel title = CreateBookmarkTitleModel(_dataService.GetBookmarkTitle(username,tconst));
        //    //CreateBookmarkTitleModel(_dataService.GetBookmarkTitle(username,tconst));
        //    var something = _dataService.GetTitleBookmarks();


        //    //if (title == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //return Ok(title);
        //    return Ok(something);
        //}

        [HttpGet("{username}/titlebookmarks")]
        public IActionResult GetBookmarksTitleaByUser(string username)
        {
            IEnumerable<BookmarkTitleModel> bookmark =
                _dataService.GetBookmarkTitlesByUser(username).Select(x => CreateBookmarkTitleModel(x));
            Console.WriteLine("IS THIS WORKING?????");

            return Ok(bookmark);
        }




        public BookmarkTitleModel CreateBookmarkTitleModel(BookmarkTitle bookmark)
        {
            var model = _mapper.Map<BookmarkTitleModel>(bookmark);
            //model.TitleUrl = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { bookmark.Username });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

            return model;
        }
        public UserModel CreateUserModel(User user)
        {
            var model = _mapper.Map<UserModel>(user);
            model.Url = _generator.GetUriByName(HttpContext, nameof(GetUser), new { user.Username });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

            return model;
        }

        [HttpPost("{username}/Rating")]
        public IActionResult CreateRating(string username, UserRatingCreateModel rating)
        {
            var rate = _mapper.Map<UserRating>(rating);
            //Console.WriteLine(rate.Rating);
            _dataService.CreateUserRating(username, rate.Tconst, rate.Rating);
            return CreatedAtRoute(null, CreateUserRatingModel(rate));
        }

        [HttpPost("{username}/search")]
        //public IActionResult CreateUserSearch(string username, UserSearchCreateModel search)
        public IActionResult CreateUserSearch(string username, string searchContent, string? searchCategory = null)
        {
            var results = _dataService.CreateUserSearch(username, searchContent, searchCategory);
            //var titleResults = results.TitleResults;
            //var test = CreateUserSearchResultsModel(results);
            return CreatedAtRoute(null, results);
        }


        public UserSearchResultsModel CreateUserSearchResultsModel(SearchResult searchResult)
        {
            var model = _mapper.Map<UserSearchResultsModel>(searchResult);
            var titleResults = searchResult.TitleResults
                .Select(x => MapTitleSearchResults(x))
                .ToList();
            var nameResults = searchResult.NameResults;
            model.TitleResults = titleResults;

            return model;
        }

        // Map tite list form DTO to WebServer model, including adding URL's
        public ListTitleModel MapTitleSearchResults(ListTitleModelDL titleBasics)
        {
            var model = new ListTitleModel().ConvertFromListTitleDTO(titleBasics);
            model.BasicTitle.Url = CreateTitleUrl(titleBasics.BasicTitle.Tconst);
            if (titleBasics.ParentTitle != null)
            {
                model.ParentTitle.Url = CreateTitleUrl(titleBasics.ParentTitle.Tconst);
            }

            return model;
        }

        public ListNameModel MapNameSearchResults(ListNameModelDL nameResults)
        {
            var model = _mapper.Map<ListNameModel>(nameResults);
            model.BasicName = _mapper.Map<BasicNameModel>(model.BasicName);
            model.BasicName.Url = _generator.GetUriByName(HttpContext, nameof(NameController.GetName), new { nameResults.BasicName.Nconst });
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




        /*
         

        public BookmarkTitleModel CreateBookmarkTitleModel(BookmarkTitle bookmark)
        {
            var model = _mapper.Map<BookmarkTitleModel>(bookmark);
            var title = _dataService.GetBasicTitle(bookmark.Tconst);
            //var something = new {TitleController }.CreateBasicTitleModel(title);
            model.Title = _mapper.Map<BasicTitleModel>(title);
            model.Title.Url = _generator.GetUriByName(HttpContext, nameof(WebServer.Controllers.TitleController.GetTitle), new { title.Tconst });

            //model.TitleUrl = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { bookmark.Username });

            return model;
        }
         */



        //public UserSearchModel CreateUserSearchModel(UserSearch search)
        public UserSearchModel CreateUserSearchModel(UserSearch search)
        {
            var model = _mapper.Map<UserSearchModel>(search);

            //model.TitleUrl = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { bookmark.Username });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

            return model;
        }



        public UserRatingModel CreateUserRatingModel(UserRating rating)
        {
            var model = _mapper.Map<UserRatingModel>(rating);
            //model.TitleUrl = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { bookmark.Username });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

            return model;
        }



        //public UserSearchModel CreateUserSearchModel(UserSearch search)
        //{
        //    var model = _mapper.Map<UserSearchModel>(search);
        //    //model.TitleUrl = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { bookmark.Username });
        //    //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

        //    return model;
        //}


        /*
         
         
         */


    }
}
