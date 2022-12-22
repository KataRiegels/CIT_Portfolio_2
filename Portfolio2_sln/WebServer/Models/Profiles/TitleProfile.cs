using AutoMapper;
using DataLayer.DomainModels.TitleModels;
using DataLayer.DTOs.TitleObjects;
using WebServer.Models.TitleModels;

namespace WebServer.Models.Profiles
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<BasicTitleDTO, BasicTitleModel>();
            CreateMap<TitleForListDTO, TitleForListModel>();
            CreateMap<DetailedTitleDTO, DetailedTitleModel>();


        }
    }
}