namespace DataLayer.DTOs.NameObjects
{
    public class DetailedNameDTO
    {
        public string Nconst { get; set; }
        public string? PrimaryName { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }

        public IList<string>? KnwonForTconst { get; set; }

        //public 



        //public IList<string>? Professions { get; set; }
        //public IList<Tuple<string, string>>? Characters { get; set; }
        ////public IList<T,T>? Characters { get; set; }
        ////public IList<KeyValuePair<string,string>>? Characters { get; set; }
        //public IList<Tuple<string, string>>? Jobs { get; set; }
    }
}