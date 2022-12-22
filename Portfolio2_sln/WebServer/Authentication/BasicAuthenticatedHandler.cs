using DataLayer.DataServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace WebServer.Authentication
{
    public class BasicAuthenticatedHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticatedHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            // Throws 401 when there was no authorization header in the request
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Console.WriteLine("1");
                return Task.FromResult(AuthenticateResult.Fail("Authentication header missing"));
            }

            var authenticationHeader = Request.Headers["Authorization"].ToString();
            var authHeaderRegex = new Regex("Basic (.*)");

            // Checks whether the authorization header contains "Basic ", which is added when using basic authentication
            if (!authHeaderRegex.IsMatch(authenticationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization code not found"));
            }


            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authenticationHeader, "$1")));

            var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");


            // If user exists in the database
            if (new DataServiceUser().GetUser(authUsername) == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("No user with this username"));
            }

            var user = new DataServiceUser().GetUser(authUsername);


            // Incorrect password
            if (authPassword != user.Password)
            {
                return Task.FromResult(AuthenticateResult.Fail("Incorrect password"));
            }


            var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, authUsername);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));


            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}
