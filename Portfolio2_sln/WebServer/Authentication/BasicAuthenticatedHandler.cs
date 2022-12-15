using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using DataLayer.DataServices;

namespace WebServer.Authentication
{
    public class BasicAuthenticatedHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticatedHandler (
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock) { 
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authentication header missing"));
            }

            var authenticationHeader = Request.Headers["Authorization"].ToString();
            var authHeaderRegex = new Regex("Basic (.*)");
             
            if (!authHeaderRegex.IsMatch(authenticationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization code not formed"));
            }

            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authenticationHeader, "$1")));

            var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

            var users = new DataServiceUser().GetUser(authUsername);
            Console.WriteLine(users.Password);

            //if (authUsername != "user" || authPassword != "password")
            //{
            //    return Task.FromResult(AuthenticateResult.Fail("Not correct"));
            //}


            if (authPassword != users.Password)
            {
                return Task.FromResult(AuthenticateResult.Fail("Not correct"));
            }



            var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, "password");
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}
