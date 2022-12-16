using AutoMapper;
using DataLayer.DataServices;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DTOs.UserObjects;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using WebServer.Authentication;
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


        [HttpGet("{username}/titlebookmarks")]
        public IActionResult GetBookmarksTitleByUser(string username)
        {
            var bookmark = _dataService.GetBookmarkTitlesByUser(username)
                .Select(x => MapTitleList(x));

            if (bookmark == null)
            {
                return NotFound();
            }

            return Ok(bookmark);
        }


        //[HttpGet("{username}/namebookmarks")]
        //public IActionResult GetBookmarksNameByUser(string username)
        //{
        //    var bookmark = _dataService.GetBookmarkNamesByUser(username)
        //        .Select(x => MapNameList(x));

        //    if (bookmark == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(bookmark);
        //}

        [HttpGet("user/namebookmarks", Name = nameof(GetBookmarksNameByUser))]
        [BasicAuthentication]
        public IActionResult GetBookmarksNameByUser()
        {

            var username = GetUserFromAuthorization();

            var bookmark = _dataService.GetBookmarkNamesByUser(username)
                .Select(x => MapNameList(x));

            if (bookmark == null)
            {
                return NotFound();
            }

            return Ok(bookmark);
        }

        private string GetUserFromAuthorization()
        {
            var authHeaderRegex = new Regex("Basic (.*)");
            string user = HttpContext.Request.Headers["Authorization"];
            Console.WriteLine(user);
            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(user, "$1")));
            var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

            Console.WriteLine("stuff" + authBase64);

            return authUsername;
        }



        //[HttpPost("user/namebookmarks")]
        //[BasicAuthentication]
        //public IActionResult CreateNameBookmark(string username, CreateBookmarkName bookmark)
        //{
        //    var result = _dataService.CreateBookmarkName(username, bookmark.Nconst, bookmark.Annotation);

        //    return CreatedAtRoute(null, result);
        //}

        /*
         
        [HttpGet("user/namebookmarks/{nconst}", Name = nameof(GetNameBookmark))]
        [BasicAuthentication]

        public IActionResult GetNameBookmark(string nconst)
        {
            var username = GetUserFromAuthorization();

            var result = _dataService.GetBookmarkName(username, nconst);


            return CreatedAtRoute(null, result);
        }

        [HttpGet("user/titlebookmarks", Name = nameof(GetTitleBookmark))]
        [BasicAuthentication]

        public IActionResult GetTitleBookmark(string tconst)
        {
            var username = GetUserFromAuthorization();

            var result = _dataService.GetBookmarkTitle(username, tconst);



            return CreatedAtRoute(null, result);
        }
         */


        [HttpPost("user/titlebookmarks")]
        [BasicAuthentication]

        public IActionResult CreateTitleBookmark(CreateBookmarkTitle bookmark)
        {
            var username = GetUserFromAuthorization();

            var result = _dataService.CreateBookmarkTitle(username, bookmark.Tconst, bookmark.Annotation);

            
            return CreatedAtRoute(null, result);
        }

        [HttpDelete("user/titlebookmarks")]
        [BasicAuthentication]

        public IActionResult DeleteTitleBookmark( string tconst)
        {
            var username = GetUserFromAuthorization();


            var result = _dataService.DeleteBookmarkTitle(username, tconst);


            return Ok(result);
        }

        [HttpDelete("user/namebookmarks")]
        [BasicAuthentication]

        public IActionResult DeleteNameBookmark(string nconst)
        {
            var username = GetUserFromAuthorization();

            var result = _dataService.DeleteBookmarkName(username, nconst);

            return Ok(result);
        }

        [HttpPost("user/ratings")]
        [BasicAuthentication]

        public IActionResult CreateRating(UserRatingCreateModel rating)
        {
            var username = GetUserFromAuthorization();

            var createdUserRating = _dataService.CreateUserRating(username, rating.Tconst, rating.Rating);

            // should have a status code in case createdUserRating is null (which would mean rating was not created)

            return CreatedAtRoute(null, CreateUserRatingModel(createdUserRating));
        }

        [HttpGet("user/ratings")]
        [BasicAuthentication]

        public IActionResult GetUserRatings()
        {

            var username = GetUserFromAuthorization();

            var rating = _dataService.GetUserRatings(username)
               .Select(x => MapUserRating(x))
                ;

            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);

        }


        private UserRatingModel MapUserRating(UserRatingDTO rating)

        {
            var model = new UserRatingModel().ConvertFromDTO(rating);
            model.TitleModel.Url = CreateTitleUrl(rating.TitleModel.Tconst);

            return model;
        }

        [HttpPost("user/searches")]
        [BasicAuthentication]

        public IActionResult CreateUserSearch(string searchContent, string? searchCategory = null)
        {
            var username = GetUserFromAuthorization();

            var results = _dataService.CreateUserSearch(username, searchContent, searchCategory);
            return CreatedAtRoute(null, results);
        }

        [HttpGet("{username}/searches")]
        public IActionResult GetUserSearches(string username)
        {
            var searches = _dataService.GetUserSearches(username)
               .Select(x => MapUserSearch(x))
                ;

            if (searches == null)
            {
                return NotFound();
            }

            return Ok(searches);

        }










        public ListNameModel MapNameList(NameForListDTO nameResults)
        {

            var model = new ListNameModel().ConvertFromListTitleDTO(nameResults);
            model.BasicName.Url = CreateTitleUrl(nameResults.BasicName.Nconst);
            if (nameResults.KnownForTitleBasics != null)
            {
                model.KnownForTitleBasics.Url = CreateTitleUrl(nameResults.KnownForTitleBasics.Tconst);
            }
            //var model = _mapper.Map<ListNameModel>(nameResults);
            //model.BasicName = _mapper.Map<BasicNameModel>(model.BasicName);
            //model.BasicName.Url = _generator.GetUriByName(HttpContext, nameof(NameController.GetName), new { nameResults.BasicName.Nconst });
            return model;
        }

        public TitleForListModel MapTitleList(TitleForListDTO titleBasics)
        {
            var model = new TitleForListModel().ConvertFromDTO(titleBasics);
            model.BasicTitle.Url = CreateTitleUrl(titleBasics.BasicTitle.Tconst);
            if (titleBasics.ParentTitle != null)
            {
                model.ParentTitle.Url = CreateTitleUrl(titleBasics.ParentTitle.Tconst);
            }

            return model;
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

       


        public UserSearchResultsModel CreateUserSearchResultsModel(SearchResultDTO searchResult)
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
        public TitleForListModel MapTitleSearchResults(TitleForListDTO titleBasics)
        {
            var model = new TitleForListModel().ConvertFromDTO(titleBasics);
            model.BasicTitle.Url = CreateTitleUrl(titleBasics.BasicTitle.Tconst);
            if (titleBasics.ParentTitle != null)
            {
                model.ParentTitle.Url = CreateTitleUrl(titleBasics.ParentTitle.Tconst);
            }

            return model;
        }

        public ListNameModel MapNameSearchResults(NameForListDTO nameResults)
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
        public UserSearchModel MapUserSearch(UserSearch search)
        {
            var model = new UserSearchModel().ConvertFromDTO(search);
            //var model = _mapper.Map<UserSearchModel>(search);
            model.Url = _generator.GetUriByName(HttpContext, nameof(SearchController.GetSearchResult), new { model.SearchContent, model.SearchCategory });

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
