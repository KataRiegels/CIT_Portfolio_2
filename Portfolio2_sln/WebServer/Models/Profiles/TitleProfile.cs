﻿using AutoMapper;
using DataLayer;
using DataLayer.Models.TitleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Models.TitleModels;

namespace WebServer.Models.Profiles
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<TitleBasics, TitleModel>();

            CreateMap<TitleCreateModel, TitleBasics>();
        }
    }
}