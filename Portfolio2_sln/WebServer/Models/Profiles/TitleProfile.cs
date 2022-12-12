using AutoMapper;
using DataLayer;
using DataLayer.Models.TitleModels;
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
            CreateMap<BasicTitleModelDL, BasicTitleModel>();
            CreateMap<ListTitleModelDL, ListTitleModel>();
            CreateMap<DetailedTitleModelDL, DetailedTitleModel>();

            CreateMap<TitleBasics, TitleModel>();

            CreateMap<TitleCreateModel, TitleBasics>();
        }
    }
}