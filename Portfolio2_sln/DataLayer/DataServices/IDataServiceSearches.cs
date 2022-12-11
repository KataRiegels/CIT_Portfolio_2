using DataLayer.DataTransferObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceSearches
    {
        SearchResult GenerateSearchResults(string searchContent, string searchCategory = null);
        SearchResult GetSearchResult(int searchId);
        //IList<ListTitleModelDL> GetFilteredTitles(List<SearchTitleModel> searchedTitles, int page = 1, int pageSize = 5);
        IList<ListTitleModelDL> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5);
    }
}