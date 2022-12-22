namespace DataLayer.DTOs.TitleObjects
{
    public class TvSeriesSeasonDTO
    {
        public string ParentTconst { get; set; }
        public int SeasonNumber { get; set; }
        public List<TvSeriesEpisodeDTO> Episodes { get; set; }


    }
}
