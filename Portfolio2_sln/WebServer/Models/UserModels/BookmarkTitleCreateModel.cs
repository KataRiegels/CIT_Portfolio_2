using WebServer.Models.TitleModels;

namespace WebServer.Models.UserModels
{
    public class BookmarkTitleCreateModel
    {

        public string? Username { get; set; }

        public string Tconst { get; set; }

        //public string? TitleUrl { get; set; }

        //public string Tconst { get; set; }
        public string? Annotation { get; set; }
    }
}
