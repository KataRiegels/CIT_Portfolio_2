using DataLayer.DTOs.NameObjects;
using WebServer.Models.TitleModels;









namespace WebServer.Models.NameModels
{
    public class NameForListModel
    {
        public string Url { get; set; }
        public BasicNameModel BasicName { get; set; }
        public BasicTitleModel? KnownForTitleBasics { get; set; } = null;

        public NameForListModel ConvertFromDTO(NameForListDTO inputModel)
        {

            return new NameForListModel()
            {
                BasicName = new BasicNameModel().ConvertFromBasicNameModelDTO(inputModel.BasicName),
                KnownForTitleBasics = new BasicTitleModel().ConvertFromDTO(inputModel.KnownForTitleBasics)
            };
        }


    }
}
