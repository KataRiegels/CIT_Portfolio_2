using DataLayer.DTOs.TitleObjects;

namespace WebServer.Models.TitleModels
{
    public class TvSeriesSeasonModel
    {
        public string ParentTconst { get; set; }
        public int SeasonNumber { get; set; }
        public List<TvSeriesEpisodeModel> Episodes { get; set; }

        public TvSeriesSeasonModel ConvertFromDTO(TvSeriesSeasonDTO inputModel)
        {

            return new TvSeriesSeasonModel()
            {
                ParentTconst = inputModel.ParentTconst,
                SeasonNumber = inputModel.SeasonNumber,
            };
        }


    }
}
