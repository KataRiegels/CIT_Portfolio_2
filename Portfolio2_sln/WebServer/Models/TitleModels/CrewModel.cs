using DataLayer.DTOs.TitleObjects;
using WebServer.Models.NameModels;

namespace WebServer.Models.TitleModels
{
    public class CrewModel
    {
        public string Tconst { get; set; }
        public BasicNameModel BasicName { get; set; }
        public BasicTitleModel BasicTitle { get; set; }

        public string Category { get; set; }
        public string CharacterName { get; set; }
        public string JobName { get; set; }

        public CrewModel ConvertFromDTO(TitleCrewDTO inputModel)
        {
            return new CrewModel
            {
                Tconst = inputModel.Tconst,
                Category = inputModel.Category,
                BasicTitle = new BasicTitleModel().ConvertFromDTO(inputModel.BasicTitle),
                BasicName = new BasicNameModel { PrimaryName = inputModel.PrimaryName },
                JobName = inputModel.JobName,
                CharacterName = inputModel.CharacterName
            };
        }

    }
}
