using DataLayer.DataTransferObjects;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.SearchObjects;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceSearches
    {
        public (IList<TitleForListDTO>, int) GenerateTitleSearchResult(string searchContent, int page = 1, int pageSize = 3);
        public (IList<NameForListDTO>, int) GeneratePersonSearchResult(string searchContent, int page, int pageSize);
        SearchResultDTO GenerateSearchResults(string searchContent, string searchCategory = null);
        SearchResultDTO GetSearchResult(int searchId);
        //IList<TitleForListDTO> GetFilteredTitles(List<TitleSearchResult> searchedTitles, int page = 1, int pageSize = 5);
        IList<TitleForListDTO> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5);
        IList<TitleForListDTO> GetFilteredTitles(List<TitleSearchResult> searchedTitles, int page = 1, int pageSize = 5);
    }
}