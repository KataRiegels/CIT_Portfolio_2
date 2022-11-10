using WebServer.Models.TitleModels;

namespace WebServer.Models.UserModels
{
    public class BookmarkTitleModel
    {
        public string Username { get; set; }
        public string? TitleUrl { get; set; }


        //public BasicTitleModel Title { get; set; }
        public string Tconst { get; set; }
        public string? Annotation { get; set; }
    }
}
