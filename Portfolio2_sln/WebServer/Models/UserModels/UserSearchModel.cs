using DataLayer.DomainModels.UserModels;

namespace WebServer.Models.UserModels
{
    public class UserSearchModel
    {
        //public string Username { get; set; }
        public string? Url { get; set; }
        public DateTime? Date { get; set; }


        public string SearchContent { get; set; }
        public string? SearchCategory { get; set; }
    
        public UserSearchModel ConvertFromDTO(UserSearch inputModel)
        {
            return new UserSearchModel
            {
                Date = inputModel.Date,
                SearchCategory = inputModel.SearchCategory,
                SearchContent = inputModel.SearchContent
            };
        }

    }
}
