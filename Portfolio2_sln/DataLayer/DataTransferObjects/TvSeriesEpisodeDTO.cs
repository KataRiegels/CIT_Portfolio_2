﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataLayer.Model;
using DataLayer.Models;



namespace DataLayer.DataTransferObjects
{
    public class TvSeriesEpisodeDTO
    {
        public string Tconst { get; set; }
        public string PrimaryTitle { get; set; }
        public int EpisodeNumber { get; set; }  


    }
}
