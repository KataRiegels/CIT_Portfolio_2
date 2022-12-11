using DataLayer.DataTransferObjects;
using DataLayer.Models.NameModels;
using DataLayer.DataTransferObjects;

namespace DataLayer.DataServices
{
    public interface IDataServiceNames
    {
        public IList<ListNameModelDL> GetFilteredNames(List<NconstObject> searchedNames, int page = 0, int pageSize = 20);
        public IList<NameTitleRelationDTO> GetNameTitleRelations(string nconst);
        BasicNameModelDL GetBasicName(string nconst);
        IList<BasicNameModelDL> GetBasicNames(int page = 0, int pageSize = 20);
        IList<DetailedNameModelDL>? GetDetailedNames(int page = 0, int pageSize = 20);
        IList<ListNameModelDL> GetListNames(int page = 0, int pageSize = 20);
        NameBasics GetName(string nconst);
        IList<NameBasics> GetNames(int page = 0, int pageSize = 20);
        public DetailedNameModelDL GetDetailedName(string nconst);



    }
}