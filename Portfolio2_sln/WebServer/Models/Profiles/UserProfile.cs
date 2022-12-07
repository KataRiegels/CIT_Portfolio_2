using AutoMapper;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Models.NameModels;
using WebServer.Models.TitleModels;
using WebServer.Models.UserModels;

namespace WebServer.Models.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<BookmarkTitle, BookmarkTitleModel>();
            CreateMap<BookmarkTitleCreateModel, BookmarkTitle>();

            CreateMap<SearchResult, UserSearchResultsModel>();

            CreateMap<User, UserModel>();
            CreateMap<UserCreateModel, User>(); 
            
            CreateMap<UserSearch, UserSearchModel>();
            CreateMap<UserSearchCreateModel, UserSearch>();
            
            CreateMap<UserRating, UserRatingModel>();
            CreateMap<UserRatingCreateModel, UserRating>();
        }
    }
}
