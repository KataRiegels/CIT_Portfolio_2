using DataLayer.DTOs.SearchObjects;
using WebServer.Models.TitleModels;

namespace WebServer.Models.UserModels
{
    public class UserSearchResultsModel
    {
        //public IList<SearchNameModel>? NameResults { get; set; }
        public IList<SearchNameModel>? NameResults { get; set; }
        //public IList<SearchTitleModel>? TitleResults { get; set; }
        public IList<ListTitleModel>? TitleResults { get; set; }
    }
}
