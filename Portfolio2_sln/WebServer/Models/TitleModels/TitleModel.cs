namespace WebServer.Models.TitleModels
{
    public class TitleModel
    {
        public string Url { get; set; }
        public string Tconst { get; set; }
        public string TitleTypes { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }

        
        public IList<string> Genres { get; set; } = new List<string>();
    }
}
