using DataLayer;
using DataLayer.DataServices;
using WebServer.Models.NameModels;

namespace TestProject

{
    public class UnitTest1
    {
#if COMMENT
        [Fact]
        public void Check_Paging()
        {
            var ds = new DataServiceNames();
            int page = 0;
            int pagesize = 20;
            var names =
                ds.GetListNames(page, pagesize).Select(x => x);

            Assert.Equal(20, names.Count());
        }
#endif



    }
}