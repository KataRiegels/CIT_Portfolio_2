using AutoMapper;
using DataLayer.DomainModels.UserModels;
using DataLayer.DTOs.SearchObjects;
using WebServer.Models.UserModels;

namespace WebServer.Models.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {

            CreateMap<User, UserModel>();
            CreateMap<UserCreateModel, User>();

            CreateMap<UserSearch, UserSearchModel>();
            CreateMap<UserSearchCreateModel, UserSearch>();

            CreateMap<UserRating, UserRatingModel>();
            CreateMap<UserRatingCreateModel, UserRating>();
        }
    }
}
