using WebServer.Models.TitleModels;
using WebServer.Models.NameModels;
using DataLayer.DTOs.SearchObjects;

namespace WebServer.Models.SearchModels
{
    public class SearchResultModel
    {
        public string? Url { get; set; }
        public IList<ListNameModel>? NameResults { get; set; }
        public IList<TitleForListModel>? TitleResults { get; set; }

        // Technically unnecessary, but here for consistency
        public SearchResultModel ConvertFromDTO(SearchResultDTO inputModel)
        {
            return new SearchResultModel();
        }




    }
}
