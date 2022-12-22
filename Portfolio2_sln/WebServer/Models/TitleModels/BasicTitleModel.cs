using DataLayer.DTOs.TitleObjects;

namespace WebServer.Models.TitleModels
{
    public class BasicTitleModel
    {
        public string? Url { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string StartYear { get; set; }


        public BasicTitleModel ConvertFromDTO(BasicTitleDTO inputModel)
        {
            if (inputModel != null)
            {

                return new BasicTitleModel()
                {
                    TitleType = inputModel.TitleType,
                    PrimaryTitle = inputModel.PrimaryTitle,
                    StartYear = inputModel.StartYear,
                };
            }

            return null;



        }
    }

}
