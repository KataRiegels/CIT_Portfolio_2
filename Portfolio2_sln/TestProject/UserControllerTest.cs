using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;


namespace TestProject
{
    public class UserControllerTest
    {

        private const string UserApi = "http://localhost:5001/api/users";
        private const string TitlesApi = "http://localhost:5001/api/titles";


        [Fact]
        public void ApiUser_GetRatings_WithAuthorizedUser()
        {
            var username = "testUser";
            var password = "p4ssW0rd";

            var (title, statusCode) = GetObject($"{UserApi}/user/ratings", username + ":" + password);

            Assert.Equal($"{UserApi}/user/ratings?page=1&pageSize=10", title["firstPageUrl"]);


            Assert.Equal(HttpStatusCode.OK, statusCode);

        }

        // Tests the attempt to get user ratings with an unauthorized user
        [Fact]
        public void ApiUser_GetRatings_WithoutAuthorizedUser()
        {
            var username = "testUser";
            var password = "incorrectP4ssW0rd";

            var (title, statusCode) = GetObject($"{UserApi}/user/ratings", username + ":" + password);


            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }



        [Fact]

        public void ApiUser_GetRatingWithAuthorizedUser_ValidTconst()
        {
            var username = "testUser";
            var password = "p4ssW0rd";

            var (title, statusCode) = GetObject($"{UserApi}/user/ratings/tt7979832", username + ":" + password);

            Assert.Equal("1", title["rating"]);
            Assert.Equal(HttpStatusCode.OK, statusCode);

        }


        [Fact]
        public void ApiUser_DeleteRating_WithAuthorizedUser()
        {
            var username = "testUser";
            var password = "p4ssW0rd";


            var (createdRating, statusCodePost) = PostData($"{UserApi}/user/ratings", new { tconst = "tt11800658", rating = 4 }, username + ":" + password);

            Assert.Equal(HttpStatusCode.Created, statusCodePost);


            var statusCodeDelete = DeleteData($"{UserApi}/user/ratings/tt11800658", username + ":" + password);

            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);



            var (rating, statusCodeGet) = GetObject($"{UserApi}/user/ratings/tt11800658", username + ":" + password);

            Assert.Equal(HttpStatusCode.NotFound, statusCodeGet);


        }




        [Fact]

        public void ApiUser_CreateRatings_WithAuthorizedUser()

        {

            var username = "testUser";
            var password = "p4ssW0rd";

            var (createdRating, statusCodePost) = PostData($"{UserApi}/user/ratings", new { tconst = "tt11800658", rating = "4" }, username + ":" + password);

            Assert.Equal(HttpStatusCode.Created, statusCodePost);
            Assert.Equal($"{TitlesApi}/tt11800658", createdRating["titleModel"]["url"]);
            Assert.Equal(4, createdRating["rating"]);


            var statusCodeDelete = DeleteData($"{UserApi}/user/ratings/tt11800658", username + ":" + password);

            Assert.Equal(HttpStatusCode.OK, statusCodeDelete);


        }







        [Fact]
        public void ApiUser_UpdateRating_WithAuthorizedUser_InvalidId()
        {
            var username = "testUser";
            var password = "p4ssW0rd";


            var statusCodePut = PutData($"{UserApi}/user/ratings/tt1180065s", new { rating = 1 }, username + ":" + password);



            Assert.Equal(HttpStatusCode.NotFound, statusCodePut);
        }


        [Fact]
        public void ApiUser_UpdateRating_WithAuthorizedUser()
        {
            var username = "testUser";
            var password = "p4ssW0rd";



            var (createdRating, statusCodePost) = PostData($"{UserApi}/user/ratings", new { tconst = "tt11800658", rating = "4" }, username + ":" + password);

            Assert.Equal(4, createdRating["rating"]);

            var statusCodePut = PutData($"{UserApi}/user/ratings/tt11800658", new { rating = 1 }, username + ":" + password);


            Assert.Equal(HttpStatusCode.Created, statusCodePut);

            var (ratingUpdated, statusCodeGetUpdated) = GetObject($"{UserApi}/user/ratings/tt11800658", username + ":" + password);

            Assert.Equal(1, ratingUpdated["rating"]);

            var statusCodeDelete = DeleteData($"{UserApi}/user/ratings/tt11800658", username + ":" + password);
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



        (JObject, HttpStatusCode) GetObject(string url, string basicAuth)
        {
            var client = new HttpClient();

            var authHeaderRegex = new Regex("Basic (.*)");


            var byteArray = Encoding.ASCII.GetBytes(basicAuth);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
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
