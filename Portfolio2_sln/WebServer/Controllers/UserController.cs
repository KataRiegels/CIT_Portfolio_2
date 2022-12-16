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
            _dataService.CreateUser(user.Username, user.Password, user.Email);
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

        /*
         
        ---- TITLE BOOKMARKS -----------
         
         */

        // Get domain object - Would be used for Uri (since it contains Tconst) 
        [HttpGet("user/titlebookmarks/domain/{tconst}")]
        [BasicAuthentication]
        public IActionResult GetTitleBookmarkObject(string tconst)
        {
            var username = GetUsernameFromAuthorization();

            var bookmark = _dataService.GetBookmarkTitle(username, tconst);

            if (bookmark == null)
                return NotFound();

            return Ok(bookmark);
        }


        // Get list of titles that active user has bookmarked
        [HttpGet("user/titlebookmarks")]
        [BasicAuthentication]
        public IActionResult GetBookmarksTitleByUser()
        {
            var username = GetUsernameFromAuthorization();

            var bookmarks = _dataService.GetBookmarkTitlesByUser(username)
                .Select(x => MapTitleList(x));
            Console.WriteLine(bookmarks.First());

            if (bookmarks == null)
                return NotFound();

            return Ok(bookmarks);
        }

        // Create bookmark 
        [HttpPost("user/titlebookmarks")]
        [BasicAuthentication]
        public IActionResult CreateTitleBookmark([FromBody] CreateBookmarkTitle bookmark)
        {
            var username = GetUsernameFromAuthorization();
            
            var createdBookmark = _dataService.CreateBookmarkTitle(username, bookmark.Tconst);
            
            // Client recieves create bookmark. If no bookmark was created, well receive null
            return CreatedAtRoute(null, createdBookmark);
        }

        // Delete bookmark
        [HttpDelete("user/titlebookmarks")]
        [BasicAuthentication]
        public IActionResult DeleteTitleBookmark(string tconst)
        {
            var username = GetUsernameFromAuthorization();

            var result = _dataService.DeleteBookmarkTitle(username, tconst);
            
            // If bookmark wasn't in the bookmark table
            if (result == -1)
                return NotFound();

            // If bookmark was still there after deletion attempt
            else if (result == 0)
                return StatusCode(500);

            return Ok(result);
        }

        // Unnecessary for our application - But all CRUD operations much be implemented (Portfolio 2 requirement)
        [HttpPut("user/titlebookmarks/{tconst}")]
        [BasicAuthentication]
        public IActionResult UpdateTitleBookmark(string tconst, [FromBody] string newTconst)
        {
            var username = GetUsernameFromAuthorization();

            var updatedBookmark = _dataService.UpdateBookmarkTitle(username, tconst, newTconst);

            if (updatedBookmark == null)
                return NotFound();

            return Ok(updatedBookmark);
        }

        /*
         
         ---------- BOOKMARK NAMES ------------
         
         */





        // Get list of people bookmarked by current user
        [HttpGet("user/namebookmarks", Name = nameof(GetBookmarksNameByUser))]
        [BasicAuthentication]
        public IActionResult GetBookmarksNameByUser()
        {
            var username = GetUsernameFromAuthorization();

            var bookmark = _dataService.GetBookmarkNamesByUser(username)
                .Select(x => MapNameList(x));

            if (bookmark == null)
                return NotFound();

            return Ok(bookmark);
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
            var username = GetUsernameFromAuthorization();

            var result = _dataService.GetBookmarkName(username, nconst);


            return CreatedAtRoute(null, result);
        }

        [HttpGet("user/titlebookmarks", Name = nameof(GetTitleBookmark))]
        [BasicAuthentication]

        public IActionResult GetTitleBookmark(string tconst)
        {
            var username = GetUsernameFromAuthorization();

            var result = _dataService.GetBookmarkTitle(username, tconst);



            return CreatedAtRoute(null, result);
        }
         */


        

        [HttpDelete("user/namebookmarks")]
        [BasicAuthentication]

        public IActionResult DeleteNameBookmark(string nconst)
        {
            var username = GetUsernameFromAuthorization();

            var result = _dataService.DeleteBookmarkName(username, nconst);

            return Ok(result);
        }

        [HttpPost("user/ratings")]
        [BasicAuthentication]

        public IActionResult CreateRating(UserRatingCreateModel rating)
        {
            var username = GetUsernameFromAuthorization();

            var createdUserRating = _dataService.CreateUserRating(username, rating.Tconst, rating.Rating);

            // should have a status code in case createdUserRating is null (which would mean rating was not created)

            return CreatedAtRoute(null, CreateUserRatingModel(createdUserRating));
        }

        [HttpGet("user/ratings")]
        [BasicAuthentication]

        public IActionResult GetUserRatings()
        {

            var username = GetUsernameFromAuthorization();

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


        /*
         * --------- USER SEARCH ------------- 
          
         */


        [HttpGet("user/searches/domain/{searchId}", Name = nameof(GetUserSearchObject))]
        public IActionResult GetUserSearchObject(int searchId)
        {

            var userSearch = _dataService.GetUserSearch(searchId);

            if (userSearch == null)
            {
                return NotFound();
            }

            return Ok(userSearch);
        }

        [HttpGet("user/searches/{searchId}", Name = nameof(GetUserSearch))]
        [BasicAuthentication]

        public IActionResult GetUserSearch(int searchId)
        {
            var username = GetUsernameFromAuthorization();

            //var title = _dataService.GetTitle(tconst);
            var userSearch = _dataService.GetUserSearch(searchId);

            if (userSearch.Username != username)
            {
                return StatusCode(401);

            }

            var result = MapUserSearch(userSearch); 
             
            if (userSearch == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost("user/searches")]
        [BasicAuthentication]

        public IActionResult CreateUserSearch([FromBody] UserSearchCreateModel userSearch)
        {
            var username = GetUsernameFromAuthorization();

            var results = _dataService.CreateUserSearch(username, userSearch.SearchContent, userSearch.SearchCategory);
            
            return CreatedAtRoute(null, results);
        }

        [HttpGet("user/searches")]
        [BasicAuthentication]

        public IActionResult GetUserSearches()
        {
            var username = GetUsernameFromAuthorization();

            var searches = _dataService.GetUserSearches(username)
               .Select(x => MapUserSearch(x))
                ;

            if (searches == null)
            {
                return NotFound();
            }

            return Ok(searches);

        }

        [HttpDelete("user/searches/{searchId}")]
        [BasicAuthentication]

        public IActionResult DeleteUserSearch(int searchId)
        {
            var username = GetUsernameFromAuthorization();

            //var title = _dataService.GetTitle(tconst);
            var userSearch = _dataService.GetUserSearch(searchId);

            // Trying to delete a search that is somehow not the logged in user
            if (userSearch.Username != username)
            {
                return StatusCode(401);
            }


            var result = _dataService.DeleteUserSearch(searchId);

            if (result == -1)
                return NotFound();
            else if (result == 0)
                return StatusCode(500);

            return Ok(result);
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

        // Gets the username based on the current user that is logged in
        private string GetUsernameFromAuthorization()
        {
            var authHeaderRegex = new Regex("Basic (.*)");
            string user = HttpContext.Request.Headers["Authorization"];
            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(user, "$1")));
            var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

            return authUsername;
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
        public object MapUserSearch(UserSearch userSearch)
        {

            var searchUrl = _generator.GetUriByName(HttpContext, nameof(GetUserSearchObject), new { userSearch.SearchId });

            var result = new
            {
                Url = searchUrl,
                SearchContent = userSearch.SearchContent,
                SearchCategory = userSearch.SearchCategory,
            };

            //var model = new UserSearchModel().ConvertFromDTO(search);
            ////var model = _mapper.Map<UserSearchModel>(search);
            //model.Url = _generator.GetUriByName(HttpContext, nameof(SearchController.GetSearchResult), new { model.SearchContent, model.SearchCategory });

            //model.TitleUrl = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { bookmark.Username });
            //model.Genres = _dataService.GetGenresFromTitle(titleBasics.Tconst);

            return result;
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
