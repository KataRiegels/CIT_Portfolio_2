using DataLayer.DataTransferObjects;
using DataLayer.DomainModels.TitleModels;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceTitles
    {

        public TvSeriesEpisodeDTO GetTvSeriesEpisode(string parentTconst, int seasonNumber, int episodeNumber);
        public int GetNumberOfTitles();
        public IList<TitleForListDTO> GetFilteredTitles(List<TconstObject> searchedTitles, int page = 1, int pageSize = 5);
        public (int, IList<TitleCrewDTO>) GetTitleCrew(string tconst, int page, int pageSize);
        BasicTitleDTO GetBasicTitle(string tconst);
        IList<BasicTitleDTO> GetBasicTitles(int page = 0, int pageSize = 20);
        DetailedTitleDTO GetDetailedTitle(string tconst);
        TitleForListDTO GetListTitle(string tconst);
        IList<TitleForListDTO> GetListTitles(int page = 0, int pageSize = 1);

        public TvSeriesSeasonDTO GetTvSeriesSeason(string tconst, int seasonNumber);
        (int, IList<TvSeriesEpisodeDTO>) GetTvSeriesEpisodes(string tconst, int seasonNumber, int page, int pageSize);
    }
}