using DataLayer.DTOs.TitleObjects;


namespace WebServer.Models.TitleModels
{
    public class TitleForListModel
    {

        public string Url { get; set; }
        public BasicTitleModel? BasicTitle { get; set; }


        // If it's a movie
        public int? Runtime { get; set; }
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }

        // If it's an episode

        public BasicTitleModel? ParentTitle { get; set; }

        public TitleForListModel ConvertFromDTO(TitleForListDTO inputModel)
        {

            //var basic = new BasicTitleModel().ConvertFromDTO(inputModel.BasicTitle);
            return new TitleForListModel()
            {
                BasicTitle = new BasicTitleModel().ConvertFromDTO(inputModel.BasicTitle),
                Runtime = inputModel.Runtime,
                Rating = inputModel.Rating,
                Genres = inputModel.Genres,
                ParentTitle = new BasicTitleModel().ConvertFromDTO(inputModel.ParentTitle)
            };
        }

        //public BasicTitleModel 

        /*
         
        public TitleForListModel ListMap(TitleForListDTO titleBasics)
        {

            //var model1 = _mapper.Map<BasicTitleModel>(titleBasics.BasicTitle);
            var _mapper = new IMapper();
            var model = _mapper.Map<TitleForListModel>(titleBasics);
            model.BasicTitle.Url = _generator.GetUriByName(HttpContext, nameof(GetTitle), new { titleBasics.BasicTitle.Tconst });
            model.BasicTitle = CreateBasicTitleModel(titleBasics.BasicTitle);

            return model;
        }
         */

        /*
        public string? Url { get; set; }
        public string Tconst { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? StartYear { get; set; }
        public string? TitleType { get; set; }
        public int? Runtime { get; set; }
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }

        // If it's an episode
        public string? ParentTconst { get; set; }
        public string? ParenTitleType { get; set; }
        public string? ParenPrimaryTitle { get; set; }
        public string? ParenStartYear { get; set; }

        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
         */


    }
}
