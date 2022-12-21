using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class UserControllerTest
    {

        private const string TitlesApi = "http://localhost:5001/api/users";


        [Fact]
        public void ApiUser_GetRatingWithAuthorizedUser_ValidTconst()
        {
            var username = "testUser";
            var password = "p4ssW0rd";

            var (title, statusCode) = GetObject($"{TitlesApi}/user/ratings/tt7979832", username + ":" + password);

            Assert.Equal("1", title["rating"]);
            Assert.Equal(HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void ApiUser_CreateRatings_WithAuthorizedUser()
        {
            var username = "testUser";
            var password = "p4ssW0rd";



            var (createdRating, statusCodePost) = PostData($"{TitlesApi}/user/ratings", new { tconst = "tt11800658", rating = "4" }, username + ":" + password);

            Assert.Equal(HttpStatusCode.Created, statusCodePost);

            var (rating, statusCodeGet) = GetObject($"{TitlesApi}/user/ratings/tt11800658", username + ":" + password);

            Assert.Equal("/api/users/user/ratings/tt11800658", rating["url"]);
            //Assert.Equal("/api/users/user/ratings/tt11800658", rating["titleModel"]["tconst"]);
            Assert.Equal("4", rating["rating"]);

            // Should technically also check whether the title's ratings changed
            /*
             */

            var statusCodeDelete = DeleteData($"{TitlesApi}/user/ratings/tt11800658", username + ":" + password);
        
        
        }

            [Fact]
            public void ApiUser_DeleteRating_WithAuthorizedUser()
            {
                var username = "testUser";
                var password = "p4ssW0rd";



                var (createdRating, statusCodePost) = PostData($"{TitlesApi}/user/ratings", new { tconst = "tt11800658", rating = "4" }, username + ":" + password);

                //Assert.Equal(HttpStatusCode.Created, statusCodePost);

                //var (rating, statusCodeGet) = GetObject($"{TitlesApi}/user/ratings/tt11800658", username + ":" + password);

                /*
                Assert.Equal("/api/users/user/ratings/tt11800658", rating["url"]);
                //Assert.Equal("/api/users/user/ratings/tt11800658", rating["titleModel"]["tconst"]);
                Assert.Equal("4", rating["rating"]);
                 */

                var statusCodeDelete = DeleteData($"{TitlesApi}/user/ratings/tt11800658", username + ":" + password);

                Assert.Equal(HttpStatusCode.OK, statusCodeDelete);



                var (rating, statusCodeGet) = GetObject($"{TitlesApi}/user/ratings/tt11800658", username + ":" + password);

                Assert.Equal(HttpStatusCode.NotFound, statusCodeGet);


            //Assert.Equal("/api/users/user/ratings/tt11800658", rating["url"]);
                //Assert.Equal("/api/users/user/ratings/tt11800658", rating["titleModel"]["tconst"]);
                //Assert.Equal("4", rating["rating"]);

            //Assert.Equal("/api/users/user/ratings?page=1&pageSize=10", title["firstPageUrl"]);


            //Assert.Equal( , title["items"]);
            //Assert.Equal(HttpStatusCode.OK, statusCode);

        }





            (JObject, HttpStatusCode) PostData(string url, object content, string basicAuth)
        {
            var client = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes(basicAuth);
            var temp = Convert.ToBase64String(byteArray);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }




        [Fact]
        public void ApiUser_GetRatings_WithAuthorizedUser()
        {
            var username = "testUser";
            var password = "p4ssW0rd";

            var (title, statusCode) = GetObject($"{TitlesApi}/user/ratings", username + ":" + password);

            Assert.Equal("/api/users/user/ratings?page=1&pageSize=10", title["firstPageUrl"]);
            
            
            //Assert.Equal( , title["items"]);
            Assert.Equal(HttpStatusCode.OK, statusCode);

        }

        // Tests the attempt to get user ratings with an unauthorized user
        [Fact]
        public void ApiUser_GetRatings_WithoutAuthorizedUser()
        {
            var username = "testUser";
            var password = "incorrectP4ssW0rd";

            var (title, statusCode) = GetObject($"{TitlesApi}/user/ratings", username + ":" + password);


            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }


        [Fact]
        public void ApiUser_UpdateRating_WithAuthorizedUser()
        {
            var username = "testUser";
            var password = "p4ssW0rd";



            var (createdRating, statusCodePost) = PostData($"{TitlesApi}/user/ratings/tt11800658", new { tconst = "tt11800658", rating = "4" }, username + ":" + password);

            Assert.Equal(HttpStatusCode.Created, statusCodePost);

            var (rating, statusCodeGet) = GetObject($"{TitlesApi}/user/ratings/tt11800658", username + ":" + password);

            Assert.Equal("/api/users/user/ratings/tt11800658", rating["url"]);
            //Assert.Equal("/api/users/user/ratings/tt11800658", rating["titleModel"]["tconst"]);
            Assert.Equal("4", rating["rating"]);

            // Should technically also check whether the title's ratings changed
            /*
             */

            var statusCodeDelete = DeleteData($"{TitlesApi}/user/ratings/tt11800658", username + ":" + password);


        }




        (JObject, HttpStatusCode) GetObject(string url, string basicAuth)
        {
            var client = new HttpClient();

            var authHeaderRegex = new Regex("Basic (.*)");


            var byteArray = Encoding.ASCII.GetBytes(basicAuth);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            //new AuthenticationHeaderValue("Basic", basicAuth);

            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }


        HttpStatusCode PutData(string url, object content, string basicAuth)
        {
            var client = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes(basicAuth);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url, string basicAuth)
        {
            var client = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes(basicAuth);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }


    }
}
