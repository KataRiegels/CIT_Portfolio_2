using AutoMapper;
using DataLayer.DomainModels.NameModels;
using DataLayer.DTOs.NameObjects;
using WebServer.Models.NameModels;

namespace WebServer.Models.Profiles
{
    public class NameProfile : Profile
    {
        public NameProfile()
        {
            CreateMap<NameBasics, NameModel>();


            CreateMap<BasicNameDTO, BasicNameModel>();
            CreateMap<NameForListDTO, NameForListModel>();

            CreateMap<DetailedNameDTO, DetailedNameModel>();
        }
    }
}