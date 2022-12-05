using DataLayer.Model;

namespace WebServer.Models.TitleModels
{
    public class BasicTitleModel
    {
        public string? Url { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string StartYear { get; set; }

        public BasicTitleModel() { }


        public BasicTitleModel ConvertBasicTitleModel(BasicTitleModelDL inputModel)
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
