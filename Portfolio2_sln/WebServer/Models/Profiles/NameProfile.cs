using AutoMapper;
using DataLayer;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Models.NameModels;

namespace WebServer.Models.Profiles
{
    public class NameProfile : Profile
    {
        public NameProfile()
        {
            CreateMap<NameBasics, NameModel>();

            CreateMap<NameCreateModel, NameBasics>();
        }
    }
}