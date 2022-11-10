﻿using DataLayer.Model;
using WebServer.Models.NameModels;

namespace WebServer.Models.TitleModels
{
    public class DetailedTitleModel
    {
        public BasicTitleModelDL BasicTitle { get; set; }
        // If it's a movie
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }
        public IList<ListNameModelDL> RelatedNames { get; set; }
        public string Plot { get; set; }

        // If it's an episode
        public BasicTitleModelDL? BasicParentTitle { get; set; }

        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
        //public string Poster { get; set; }



    }
}
