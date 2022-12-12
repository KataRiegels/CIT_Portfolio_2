using DataLayer.DataTransferObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.Models.TitleModels;

namespace DataLayer.DataServices
{
    public interface IDataServiceTitles
    {
        public IList<ListTitleModelDL> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5);
        public IList<TitleCastDTO> GetTitleCrew(string tconst);
        public IList<TitleCastDTO> GetTitleCast(string tconst);
        BasicTitleModelDL GetBasicTitle(string tconst);
        IList<BasicTitleModelDL> GetBasicTitles(int page = 0, int pageSize = 20);
        DetailedTitleModelDL GetDetailedTitle(string tconst);
        IList<DetailedTitleModelDL>? GetDetailedTitles(int page, int pageSize);
        ListTitleModelDL GetListTitle(string tconst);
        IList<ListTitleModelDL> GetListTitles(int page = 0, int pageSize = 1);
        TitleBasics GetTitle(string tconst);
        IList<TitleBasics> GetTitles(int page = 0, int pageSize = 20);

        public TvSeriesSeasonDTO GetTvSeriesSeason(string tconst, int seasonNumber);
    }
}