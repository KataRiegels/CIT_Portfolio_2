using DataLayer.DTOs.NameObjects;

namespace WebServer.Models.NameModels
{
    public class BasicNameModel
    {
        public string Url { get; set; }
        public string PrimaryName { get; set; }

        public BasicNameModel ConvertFromBasicNameModelDTO(BasicNameDTO inputModel)
        {
            return new BasicNameModel
            {
                PrimaryName = inputModel.PrimaryName
            };
        }


    }
}
