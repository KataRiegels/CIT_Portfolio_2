using DataLayer.Model;

namespace WebServer.Models.UserModels
{
    public class UserSearchResultsModel
    {
        public IList<SearchNameModel>? NameResults { get; set; }
        public IList<SearchTitleModel>? TitleResults { get; set; }
    }
}
