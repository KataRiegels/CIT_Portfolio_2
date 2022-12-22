using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DTOs.SearchObjects
{
    public class SearchResultDTO
    {
        public int SearchId { get; set; }
        public IList<NameForListDTO>? NameResults { get; set; }
        public IList<TitleForListDTO>? TitleResults { get; set; }

    }
}
