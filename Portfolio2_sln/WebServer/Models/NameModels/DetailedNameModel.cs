namespace WebServer.Models.NameModels
{
    public class DetailedNameModel
    {
        public string Url { get; set; }
        public string Nconst { get; set; }
        public string? PrimaryName { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public IList<string>? Professions { get; set; }
        public IList<string>? KnwonForTconst { get; set; }
        public IList<Tuple<string, string>>? Characters { get; set; }
        //public IList<IList<string>>? Characters { get; set; }
        public IList<Tuple<string, string>>? Jobs { get; set; }
    }
}
