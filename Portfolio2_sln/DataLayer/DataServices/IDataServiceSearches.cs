using DataLayer.DataTransferObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceSearches
    {
        SearchResult GenerateSearchResults(string searchContent, string searchCategory = null);
        IList<ListNameModelDL> GetFilteredNames(List<SearchNameModel> searchedNames, int page = 1, int pageSize = 5);
        SearchResult GetSearchResult(int searchId);
        IList<ListTitleModelDL> GetFilteredTitles(List<SearchTitleModel> searchedTitles, int page = 1, int pageSize = 5);
    }
}