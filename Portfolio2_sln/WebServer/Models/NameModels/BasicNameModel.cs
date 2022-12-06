using DataLayer.Model;

namespace WebServer.Models.NameModels
{
    public class BasicNameModel
    {
        public string Url { get; set; }
        public string PrimaryName { get; set; }

        public BasicNameModel ConvertFromBasicNameModelDTO(BasicNameModelDL inputModel)
        {
            return new BasicNameModel
            {
                PrimaryName = inputModel.PrimaryName
            };
        }


    }
}
