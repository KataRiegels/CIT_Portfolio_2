using DataLayer;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebServer.Models.NameModels;

namespace TestProject

{
    public class WebServiceTest
    {

        private const string TitlesApi = "http://localhost:5001/api/titles";
        private const string NamesApi = "http://localhost:5001/api/names";

        // Test whether HttpGet {tconst} works and returns correct title
        [Fact]
        public void ApiTitles_GetWithValidTconst_OkAndTitle()
        {
            var (category, statusCode) = GetObject($"{TitlesApi}/tt10850888");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("My Country: The New Age", category["primaryTitle"]);
        }

        // Tests the "detailed" route and checks that genres have correctly been added
        [Fact]
        public void ApiTitles_GetDetailedWithValidTconst_OkAndGenres()
        {
            
            var (category, statusCode) = GetObject($"{TitlesApi}/detailed/tt10458336");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(JArray.Parse("[\"Crime\", \"Drama\", \"Mystery\"]"), category["genres"]);


        }

        //["Crime", "Drama", "Mystery"]


        // Helpers

        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        HttpStatusCode PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }

    }
}