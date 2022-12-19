using DataLayer.DataTransferObjects;
using DataLayer.DomainModels.NameModels;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceNames
    {
        public NameForListDTO GetListName(string nconst);
        public (int, IList<TitleCrewDTO>) GetRelatedTitles(string nconst, int page, int pageSize);

        public IList<NameForListDTO> GetFilteredNames(List<NconstObject> searchedNames, int page = 0, int pageSize = 20);
        //public IList<NameTitleRelationDTO> GetNameTitleRelations(string nconst);
        BasicNameDTO GetBasicName(string nconst);
        IList<BasicNameDTO> GetBasicNames(int page = 0, int pageSize = 20);
        IList<DetailedNameDTO>? GetDetailedNames(int page = 0, int pageSize = 20);
        IList<NameForListDTO> GetListNames(int page = 0, int pageSize = 20);
        NameBasics GetName(string nconst);
        IList<NameBasics> GetNames(int page = 0, int pageSize = 20);
        public DetailedNameDTO GetDetailedName(string nconst);
        int GetNumberOfPeople();
    }
}