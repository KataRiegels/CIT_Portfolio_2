using System.Security.Cryptography.X509Certificates;
using DataLayer.Models;
using WebServer.Controllers;
using WebServer.Models.NameModels;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            [Fact]
            Public void Check_Paging()
            {
                int page = 0;
                int pagesize = 20;
                IEnumerable<ListNameModel> names =
                    _dataService.GetListNames(page, pagesize).Select(x => x);

                Assert.Equals(20, names.Count());
            }

        }
    }
}