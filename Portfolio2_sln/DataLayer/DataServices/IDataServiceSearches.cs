using DataLayer.DataTransferObjects;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceSearches
    {
        SearchResultDTO GenerateSearchResults(string searchContent, string searchCategory = null);
        SearchResultDTO GetSearchResult(int searchId);
        //IList<TitleForListDTO> GetFilteredTitles(List<TitleSearchResult> searchedTitles, int page = 1, int pageSize = 5);
        IList<TitleForListDTO> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5);
    }
}