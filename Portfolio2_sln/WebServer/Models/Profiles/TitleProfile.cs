using AutoMapper;
using DataLayer;
using DataLayer.DomainModels.TitleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Models.TitleModels;
using DataLayer.DTOs.TitleObjects;

namespace WebServer.Models.Profiles
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<BasicTitleDTO, BasicTitleModel>();
            CreateMap<TitleForListDTO, TitleForListModel>();
            CreateMap<DetailedTitleDTO, DetailedTitleModel>();

            CreateMap<TitleBasics, TitleModel>();

        }
    }
}