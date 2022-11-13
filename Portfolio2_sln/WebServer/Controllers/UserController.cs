﻿using AutoMapper;
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
        [HttpGet("bookmark")]
        //public IActionResult GetBookmarkTitle(string username, string tconst)
        public IActionResult GetBookmarkTitle()
        {
            //BookmarkTitleModel title = CreateBookmarkTitleModel(_dataService.GetBookmarkTitle(username,tconst));
            //CreateBookmarkTitleModel(_dataService.GetBookmarkTitle(username,tconst));
            var something = _dataService.GetTitleBookmarks();


            //if (title == null)
            //{
            //    return NotFound();
            //}
            //return Ok(title);
            return Ok(something);
        }

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

        /*
         
         
         */


    }
}