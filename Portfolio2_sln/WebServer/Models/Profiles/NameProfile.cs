﻿using AutoMapper;
using DataLayer;
using DataLayer.DTOs.NameObjects;
using DataLayer.DomainModels.NameModels;
using DataLayer.DomainModels.TitleModels;
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


            CreateMap<BasicNameDTO, BasicNameModel>();
            CreateMap<NameForListDTO, NameForListModel>();

            CreateMap<DetailedNameDTO, DetailedNameModel>();
        }
    }
}