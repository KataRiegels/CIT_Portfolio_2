namespace DataLayer.DTOs.TitleObjects
{
    public class TitleForListDTO
    {

        public BasicTitleDTO? BasicTitle { get; set; }


        // If it's a movie/tv series ....
        public int? Runtime { get; set; }
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }

        // If it's an episode

        public BasicTitleDTO? ParentTitle { get; set; }

    }
}
