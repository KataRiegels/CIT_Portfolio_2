using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class ListTitleModelDL
    {
        //public BasicTitleModelDL BasicTitle { get; set; }

        public BasicTitleModelDL? BasicTitle { get; set; }
        //public string? PrimaryTitle { get; set; }
        //public string? StartYear { get; set; }
        //public string? TitleType { get; set; }
        //public string Tconst { get; set; }
        
        
        // If it's a movie
        public int? runtime { get; set; }
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }

        // If it's an episode

        public ListTitleModelDL? ParentTitle { get; set; }

        //public string? ParentTconst { get; set; }
        //public string? ParentTitleType { get; set; }
        //public string? ParentPrimaryTitle { get; set; }
        //public string? ParentStartYear { get; set; }

        //public int? SeasonNumber { get; set; }
        //public int? EpisodeNumber { get; set; }
    }
}
