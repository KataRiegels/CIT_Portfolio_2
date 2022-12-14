using DataLayer.DTOs.SearchObjects;
using WebServer.Models.NameModels;
using WebServer.Models.TitleModels;

namespace WebServer.Models.SearchModels
{
    public class SearchResultModel
    {
        public string? Url { get; set; }
        public IList<NameForListModel>? NameResults { get; set; }
        public IList<TitleForListModel>? TitleResults { get; set; }

        // Technically unnecessary, but here for consistency
        public SearchResultModel ConvertFromDTO(SearchResultDTO inputModel)
        {
            return new SearchResultModel();
        }




    }
}
