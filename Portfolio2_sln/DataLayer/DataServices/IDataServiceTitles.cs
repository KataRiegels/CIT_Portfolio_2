using DataLayer.DataTransferObjects;
using DataLayer.DTOs.TitleObjects;
using DataLayer.DomainModels.TitleModels;

namespace DataLayer.DataServices
{
    public interface IDataServiceTitles
    {

        public TvSeriesEpisodeDTO GetTvSeriesEpisode(string parentTconst, int seasonNumber, int episodeNumber);
        public int GetNumberOfTitles();
        public IList<TitleForListDTO> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5);
        public (int, IList<TitleCrewDTO>) GetTitleCrew(string tconst, int page, int pageSize);
        public IList<TitleCastDTO> GetTitleCast(string tconst);
        BasicTitleDTO GetBasicTitle(string tconst);
        IList<BasicTitleDTO> GetBasicTitles(int page = 0, int pageSize = 20);
        DetailedTitleDTO GetDetailedTitle(string tconst);
        IList<DetailedTitleDTO>? GetDetailedTitles(int page, int pageSize);
        TitleForListDTO GetListTitle(string tconst);
        IList<TitleForListDTO> GetListTitles(int page = 0, int pageSize = 1);
        TitleBasics GetTitle(string tconst);
        IList<TitleBasics> GetTitles(int page = 0, int pageSize = 20);

        public TvSeriesSeasonDTO GetTvSeriesSeason(string tconst, int seasonNumber);
        (int, IList<TvSeriesEpisodeDTO>) GetTvSeriesEpisodes(string tconst, int seasonNumber, int page, int pageSize);
    }
}