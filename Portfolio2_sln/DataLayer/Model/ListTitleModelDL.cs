﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class ListTitleModelDL
    {
        //public BasicTitleModelDL BasicTitle { get; set; }
        // If it's a movie

        public string Tconst { get; set; }
        public string TitleType { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? StartYear { get; set; }

        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }

        // If it's an episode
        public BasicTitleModelDL? BasicParentTitle { get; set; }

        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
    }
}
