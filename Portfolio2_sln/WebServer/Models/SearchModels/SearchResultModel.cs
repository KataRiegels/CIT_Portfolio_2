using WebServer.Models.TitleModels;
using WebServer.Models.NameModels;
using DataLayer.DTOs.SearchObjects;

namespace WebServer.Models.SearchModels
{
    public class SearchResultModel
    {
        public string? Url { get; set; }
        public IList<ListNameModel>? NameResults { get; set; }
        public IList<ListTitleModel>? TitleResults { get; set; }

        // Technically unnecessary, but here for consistency
        public SearchResultModel ConvertFromSearchResultDTO(SearchResult inputModel)
        {
            
            return new SearchResultModel();

        }




    }
}
