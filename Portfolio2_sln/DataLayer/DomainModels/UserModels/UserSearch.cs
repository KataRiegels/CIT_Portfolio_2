namespace DataLayer.DomainModels.UserModels
{
    public class UserSearch
    {
        //public string UserId { get; set; }
        public string? Username { get; set; }

        public int SearchId { get; set; }
        public DateTime? Date { get; set; }
        public string? SearchContent { get; set; }
        public string? SearchCategory { get; set; }




    }
}
