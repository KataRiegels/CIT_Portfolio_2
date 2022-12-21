using DataLayer;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebServer.Models.NameModels;
using DataLayer.DataServices;
using Moq;
using DataLayer.DomainModels.TitleModels;
using WebServer.Controllers;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using System.Reflection.Emit;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;

namespace TestProject

{
    public class WebServiceTest
    {
        //private Mock<IRepository<Comment>> _mockRepository;
        //private Mock<IDataServiceTitles> _mockRepository;
        //private IDataServiceTitles _service;
        ////private ModelStateDictionary _modelState;

        //public WebServiceTest()
        //{
        //    //_modelState = new ModelStateDictionary();
        //    _mockRepository = new Mock<IDataServiceTitles>();
        //    //_service = new IDataServiceTitles(new ModelStateWrapper(_modelState), _mockRepository.Object);
        //}

        private const string TitlesApi = "http://localhost:5001/api/titles";
        private const string NamesApi = "http://localhost:5001/api/names";

        // Test whether HttpGet {tconst} works and returns correct title
        //[Fact]
        public void ApiTitles_GetWithValidTconst_OkAndTitle()
        {
            //var (category, statusCode) = GetObject($"{TitlesApi}/tt10850888, ");

            //Assert.Equal(HttpStatusCode.OK, statusCode);
            //Assert.Equal("My Country: The New Age", category["primaryTitle"]);
        }

        // Tests the "detailed" route and checks that genres have correctly been added
        [Fact]
        public void ApiTitles_GetDetailedWithValidTconst_OkAndGenres()
        {

            //var (title, statusCode) = GetObject($"{TitlesApi}/detailed/tt11156314", "dGVzdFVzZXI6cDRzc1cwcmQ=");
            var (title, statusCode) = GetObject($"{TitlesApi}/detailed/tt11156314", "testUser:p4ssW0rds");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(JArray.Parse("[\"Adventure\", \"Animation\", \"Comedy\"]"), title["genres"]);
            //Assert.Equal("http://localhost:5001/titles/tt11156314", title["url"]);
            Assert.Equal("/api/titles/detailed/tt11156314", title["url"]);
            Assert.Equal("The Curse/First Day Frights", title["primaryTitle"] );
            Assert.Equal("2021", title["startYear"] );
            Assert.Equal("tvEpisode", title["titleType"] );
            Assert.Equal("23", title["runtime"] );
            Assert.Equal( "8.1", title["rating"] );
            Assert.Equal("After moving into a new town, Molly encounters Scratch who decides to put a curse on Molly for entering his home to try and get the McGee family to flee.", title["plot"] );
            Assert.Equal("https://m.media-amazon.com/images/M/MV5BOTVmODFhZGUtZTgxYS00MWE5LTlkYTItYzJkNjQ1MWQ3ZTJjXkEyXkFqcGdeQXVyMTEzMTI1Mjk3._V1_SX300.jpg", title["poster"]);
        }


        (JObject, HttpStatusCode) GetObject(string url, string basicAuth)
        {
            var client = new HttpClient();

            var authHeaderRegex = new Regex("Basic (.*)");


            var byteArray = Encoding.ASCII.GetBytes(basicAuth);
            var temp = Convert.ToBase64String(byteArray);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                //new AuthenticationHeaderValue("Basic", basicAuth);

            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }


        [Fact]
        public void test()
        {
            var ds = new DataServiceTitles();

            var result = ds.GetListTitles();

            Assert.Equal(1, result.Count);

        }

        [Fact]
        public void test2()
        {
            var ds = new DataServiceTitles();

            var result = ds.GetListTitles(1, 5);

            Assert.Equal(5, result.Count);

        }

        [Fact]
        public void test3()
        {
            var ds = new DataServiceTitles();

            var result = ds.GetListTitles();

            //Assert.Equal(1, result.First().BasicTitle.Tconst);

        }



        //public TitleController(IDataServiceTitles dataService, LinkGenerator generator)
        //{
        //    var _dataService = dataService;
        //    var _generator = generator;
        //}


        //(JObject, HttpStatusCode) GetObject(string url)
        //{
        //    var client = new HttpClient();
        //    var response = client.GetAsync(url).Result;
        //    var data = response.Content.ReadAsStringAsync().Result;
        //    return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        //}


        /*
         

        [Fact]
        public void GetReturnsProductWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IDataServiceTitles>();
            var mockLinkGenerator = new Mock<LinkGenerator>();
            var mockMapper = new Mock<IMapper>();

            mockRepository.Setup(x => x.GetTitle("tt10458336"))
                .Returns(new TitleBasics { Tconst = "tt10458336" });
            //mockLinkGenerator.Setup(x => x.GetUriByName(new DefaultHttpContext(), nameof(), new { }));


            var controller = new TitleController(mockRepository.Object, mockLinkGenerator.Object, mockMapper.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            // Act
            var actionResult = controller.GetTitle("tt10458336");

            var contentResult = actionResult as ObjectResult;



            var test = contentResult.StatusCode;
            var temp = controller.StatusCode((int)contentResult.StatusCode).StatusCode;
            var tempt2 = HttpStatusCode.OK;
            Assert.Equal(StatusCodes.Status200OK, (int)contentResult.StatusCode);


            // Assert
            //Assert.NotNull(contentResult);
            //Assert.NotNull(contentResult.Content);
            //Assert.Equal(42, contentResult.Content.Tconst);
        }


         */


        //["Crime", "Drama", "Mystery"]


        // Helpers

        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
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