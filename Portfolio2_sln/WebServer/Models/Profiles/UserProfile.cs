using AutoMapper;
using DataLayer;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DomainModels.NameModels;
using DataLayer.DomainModels.TitleModels;
using DataLayer.DomainModels.UserModels;
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

            CreateMap<SearchResultDTO, UserSearchResultsModel>();

            CreateMap<User, UserModel>();
            CreateMap<UserCreateModel, User>(); 
            
            CreateMap<UserSearch, UserSearchModel>();
            CreateMap<UserSearchCreateModel, UserSearch>();
            
            CreateMap<UserRating, UserRatingModel>();
            CreateMap<UserRatingCreateModel, UserRating>();
        }
    }
}
