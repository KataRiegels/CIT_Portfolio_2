using DataLayer.DTOs.TitleObjects;

namespace WebServer.Models.TitleModels
{
    public class TvSeriesEpisodeModel
    {
        public string Url { get; set; }
        public string PrimaryTitle { get; set; }
        public int EpisodeNumber { get; set; }

        public TvSeriesEpisodeModel ConvertFromDTO(TvSeriesEpisodeDTO inputModel)
        {
            return new TvSeriesEpisodeModel()
            {
                PrimaryTitle = inputModel.PrimaryTitle,
                EpisodeNumber = inputModel.EpisodeNumber,
            };
        }


    }
}
