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

            var episodes = Episodes;

            var result = new TvSeriesSeasonModel()
            {
                ParentTconst = inputModel.ParentTconst,
                SeasonNumber = inputModel.SeasonNumber,
                //Episodes = (new TvSeriesEpisodeModel().ConvertFromDtoObject(inputModel.Episodes))
                //PrimaryTitle = inputModel.PrimaryTitle,
                //EpisodeNumber = inputModel.EpisodeNumber,
            };

            foreach (var episode in inputModel.Episodes)
            {

            }
            return new TvSeriesSeasonModel()
            {
                ParentTconst = inputModel.ParentTconst,
                SeasonNumber = inputModel.SeasonNumber,
                //Episodes = (new TvSeriesEpisodeModel().ConvertFromDtoObject(inputModel.Episodes))
                //PrimaryTitle = inputModel.PrimaryTitle,
                //EpisodeNumber = inputModel.EpisodeNumber,
            };
        }


    }
}
