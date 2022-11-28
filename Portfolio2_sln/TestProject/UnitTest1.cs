using DataLayer;
using WebServer.Models.NameModels;

namespace TestProject

{
    public class UnitTest1
    {
        [Fact]
        public void Check_Paging()
        {
            var ds = new DataService();
            int page = 0;
            int pagesize = 20;
            var names =
                ds.GetListNames(page, pagesize).Select(x => x);

            Assert.Equal(20, names.Count());
        }
    }
}