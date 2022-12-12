using DataLayer.DTOs.TitleObjects;

namespace WebServer.Models.TitleModels
{
    public class DetailedTitleModel
    {
        public string Url { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? StartYear { get; set; }
        public string? TitleType { get; set; }
        public int? Runtime { get; set; }
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }
        public string? Plot { get; set; }
        public string? Poster { get; set; }
        public string? relatedName { get; set; }


        public DetailedTitleModel ConvertFromDetailedTitleDTO(DetailedTitleModelDL inputModel)
        {
            var model = new DetailedTitleModel()
            {
                //Tconst = inputModel.Tconst,
                PrimaryTitle = inputModel.PrimaryTitle,
                StartYear = inputModel.StartYear,
                TitleType = inputModel.TitleType,
                Runtime = inputModel.Runtime,
                Rating = inputModel.Rating,
                Genres = inputModel.Genres,
                Plot = inputModel.Plot,
                Poster = inputModel.Poster
            };

            return model;
        }


    }
}
