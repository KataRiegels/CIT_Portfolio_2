using DataLayer.Model;
using DataLayer.Models.NameModels;

namespace DataLayer.DataServices
{
    public interface IDataServiceNames
    {
        BasicNameModelDL GetBasicName(string nconst);
        IList<BasicNameModelDL> GetBasicNames(int page = 0, int pageSize = 20);
        IList<DetailedNameModelDL>? GetDetailedNames(int page = 0, int pageSize = 20);
        IList<ListNameModelDL> GetListNames(int page = 0, int pageSize = 20);
        NameBasics GetName(string nconst);
        IList<NameBasics> GetNames(int page = 0, int pageSize = 20);
    }
}