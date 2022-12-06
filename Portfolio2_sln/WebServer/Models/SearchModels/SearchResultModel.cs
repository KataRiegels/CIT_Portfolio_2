using DataLayer.Model;
using WebServer.Models.TitleModels;
using WebServer.Models.NameModels;

namespace WebServer.Models.SearchModels
{
    public class SearchResultModel
    {
        public string? Url { get; set; }
        public IList<ListNameModel>? NameResults { get; set; }
        public IList<ListTitleModel>? TitleResults { get; set; }

        public SearchResultModel ConvertFromSearchResultDTO(SearchResult inputModel)
        {
            //var convertedModel = new SearchResultModel();
            
            
            return new SearchResultModel();

        }




    }
}
