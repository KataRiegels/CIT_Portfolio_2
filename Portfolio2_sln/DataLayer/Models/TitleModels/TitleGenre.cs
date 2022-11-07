namespace DataLayer.Models.TitleModels
{
    public class TitleGenre
    {
        public string Tconst { get; set; }
        public string GenreName { get; set; }

        //public TitleBasics Title { get; set; }
        public IList<TitleBasics> TitleBasics { get; set; } = new List<TitleBasics>();

    }
}
