using Microsoft.AspNetCore.Authorization;


namespace WebServer.Authentication
{
    public class AdminAuthentiction : AuthorizeAttribute
    {
        public AdminAuthentiction()
        {
            Policy = "AdminAuthentication";

        }
    }
}
