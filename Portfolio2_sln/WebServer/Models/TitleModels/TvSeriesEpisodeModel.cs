using DataLayer.DTOs.TitleObjects;

namespace WebServer.Models.TitleModels
{
    public class TvSeriesEpisodeModel
    {
        public string Url { get; set; }
        public string PrimaryTitle { get; set; }
        public string ParentTconst { get; set; }
        public int EpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }

        public TvSeriesEpisodeModel ConvertFromDTO(TvSeriesEpisodeDTO inputModel)
        {
            return new TvSeriesEpisodeModel()
            {
                ParentTconst = inputModel.ParentTconst,
                PrimaryTitle = inputModel.PrimaryTitle,
                EpisodeNumber = inputModel.EpisodeNumber,
                SeasonNumber = inputModel.SeasonNumber,

            };
        }


    }
}
