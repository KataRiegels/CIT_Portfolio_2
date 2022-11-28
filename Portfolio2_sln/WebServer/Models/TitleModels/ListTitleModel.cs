





using DataLayer.Model;

namespace WebServer.Models.TitleModels
{
    public class ListTitleModel
    {

        public BasicTitleModel? BasicTitle { get; set; }


        // If it's a movie
        public int? runtime { get; set; }
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }

        // If it's an episode

        public ListTitleModel? ParentTitle { get; set; }

        /*
         
        public string? Url { get; set; }
        public string Tconst { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? StartYear { get; set; }
        public string? TitleType { get; set; }
        public int? runtime { get; set; }
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }

        // If it's an episode
        public string? ParentTconst { get; set; }
        public string? ParenTitleType { get; set; }
        public string? ParenPrimaryTitle { get; set; }
        public string? ParenStartYear { get; set; }

        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
         */


    }
}
