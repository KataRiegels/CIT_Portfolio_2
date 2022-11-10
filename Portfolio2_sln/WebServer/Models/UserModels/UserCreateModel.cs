namespace WebServer.Models.UserModels
{
    public class UserCreateModel
    {
        public string? Url { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string? BirthYear { get; set; }
        public string Email { get; set; }

    }
}
