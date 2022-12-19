using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataLayer.Model;
using DataLayer.DomainModels;



namespace DataLayer.DTOs.TitleObjects
{
    public class TvSeriesEpisodeDTO
    {
        public string Tconst { get; set; }
        public string PrimaryTitle { get; set; }
        public string ParentTconst { get; set; }
        public int EpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }


    }
}
