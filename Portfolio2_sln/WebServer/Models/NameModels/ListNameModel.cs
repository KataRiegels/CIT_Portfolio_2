using DataLayer.DTOs.NameObjects;
using WebServer.Models.TitleModels;

namespace WebServer.Models.NameModels
{
    public class ListNameModel
    {
        public BasicNameModel BasicName { get; set; }
        public BasicTitleModel? KnownForTitleBasics { get; set; } = null;

        public ListNameModel ConvertFromListTitleDTO(NameForListDTO inputModel)
        {

            return new ListNameModel()
            {
                BasicName = new BasicNameModel().ConvertFromBasicNameModelDTO(inputModel.BasicName),
                KnownForTitleBasics = new BasicTitleModel().ConvertBasicTitleModel(inputModel.KnownForTitleBasics)
            };
        }


    }
}
