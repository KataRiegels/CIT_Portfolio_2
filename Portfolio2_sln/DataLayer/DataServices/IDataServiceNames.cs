using DataLayer.DataTransferObjects;
using DataLayer.Models.NameModels;
using DataLayer.DTOs.NameObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceNames
    {
        public IList<NameForListDTO> GetFilteredNames(List<NconstObject> searchedNames, int page = 0, int pageSize = 20);
        public IList<NameTitleRelationDTO> GetNameTitleRelations(string nconst);
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