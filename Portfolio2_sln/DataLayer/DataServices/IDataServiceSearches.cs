using DataLayer.Model;

namespace DataLayer.DataServices
{
    public interface IDataServiceSearches
    {
        SearchResult GenerateSearchResults(string searchContent, string searchCategory = null);
        IList<ListNameModelDL> GetNamesForSearch(List<SearchNameModel> searchedNames, int page = 1, int pageSize = 5);
        SearchResult GetSearchResult(int searchId);
        IList<ListTitleModelDL> GetTitlesForSearch(List<SearchTitleModel> searchedTitles, int page = 1, int pageSize = 5);
    }
}