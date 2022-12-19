using DataLayer.DomainModels.UserModels;
using System.Security.Principal;

namespace WebServer.Authentication
{
    public class AuthenticatedUser : IIdentity
    {
        public AuthenticatedUser(
            string authenticationType,
            bool isAuthenticated,
            string name
            )
        {
            AuthenticationType = authenticationType;
            IsAuthenticated = IsAuthenticated;
            Name = name;
        }

        public string? AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? Name { get; set; }
    }
}
