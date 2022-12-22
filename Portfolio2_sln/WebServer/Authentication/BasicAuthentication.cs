using Microsoft.AspNetCore.Authorization;

namespace WebServer.Authentication
{
    public class BasicAuthentication : AuthorizeAttribute

    {
        public BasicAuthentication()
        {
            Policy = "BasicAuthentication";
        }

    }
}
