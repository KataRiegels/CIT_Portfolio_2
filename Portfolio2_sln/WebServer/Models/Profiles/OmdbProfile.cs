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
using WebServer.Models.TitleModels;

namespace WebServer.Models.Profiles
{
    public class OmdbProfile : Profile
    {
        public OmdbProfile()
        {
            CreateMap<OmdbData, OmdbModel>();

            CreateMap<OmdbCreateModel, OmdbData>();
        }
    }
}
