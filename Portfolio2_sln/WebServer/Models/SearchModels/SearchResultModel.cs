using DataLayer.Model;
using WebServer.Models.TitleModels;

namespace WebServer.Models.SearchModels
{
    public class SearchResultModel
    {
        public string? Url { get; set; }
        public IList<SearchNameModel>? NameResults { get; set; }
        //public IList<SearchTitleModel>? TitleResults { get; set; }
        public IList<ListTitleModel>? TitleResults { get; set; }

        public SearchResultModel ConvertFromSearchResultDTO(SearchResult inputModel)
        {
            //var basic = new BasicTitleModel().ConvertBasicTitleModel(inputModel.BasicTitle);
            //var titleResults = inputModel.TitleResults
            //    .Select(x => new ListTitleModel().ConvertFromListTitleDTO(x))
            //    .ToList();


            //convertedModel.TitleResults = titleResults;
            var convertedModel = new SearchResultModel();
            //convertedModel.TitleResults = inputModel.TitleResults;
            
            
            return convertedModel;

            //return new ListTitleModel()
            //{
            //    BasicTitle = new BasicTitleModel().ConvertBasicTitleModel(inputModel.BasicTitle),
            //    Runtime = inputModel.Runtime,
            //    Rating = inputModel.Rating,
            //    Genres = inputModel.Genres,
            //    ParentTitle = new BasicTitleModel().ConvertBasicTitleModel(inputModel.ParentTitle)




            //};
        }




    }
}
