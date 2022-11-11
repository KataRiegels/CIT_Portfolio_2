using AutoMapper;
using DataLayer;
using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Models.NameModels;
using WebServer.Models.TitleModels;

namespace WebServer.Models.Profiles
{
    public class NameProfile : Profile
    {
        public NameProfile()
        {
            CreateMap<NameBasics, NameModel>();

            CreateMap<NameCreateModel, NameBasics>();

            CreateMap<BasicNameModelDL, BasicNameModel>();
            CreateMap<ListNameModelDL, ListNameModel>();

            CreateMap<DetailedNameModelDL, DetailedNameModel>();
        }
    }
}