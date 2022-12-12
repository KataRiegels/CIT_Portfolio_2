using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.TitleModels
{
    public class TitleEpisode
    {
        public string Tconst { get; set; }
        public string ParentTconst { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
    }
}
