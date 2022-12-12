using DataLayer.DTOs.SearchObjects;
using WebServer.Models.TitleModels;

namespace WebServer.Models.UserModels
{
    public class UserSearchResultsModel
    {
        //public IList<NameSearchResult>? NameResults { get; set; }
        public IList<NameSearchResult>? NameResults { get; set; }
        //public IList<TitleSearchResult>? TitleResults { get; set; }
        public IList<TitleForListModel>? TitleResults { get; set; }
    }
}
