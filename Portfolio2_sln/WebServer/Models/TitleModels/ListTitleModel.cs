namespace WebServer.Models.TitleModels
{
    public class ListTitleModel
    {
        public BasicTitleModel BasicTitle { get; set; }
        // If it's a movie
        public double? Rating{ get; set; }
        public IList<string>? Genres { get; set; }

        // If it's an episode
        public BasicTitleModel? BasicParentTitle { get; set; }

        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }


    }
}
