
using DataLayer.DataTransferObjects;
using WebServer.Models.NameModels;

namespace WebServer.Models.TitleModels
{
    public class CrewModel
    {
        public string Tconst { get; set; }
        public BasicNameModel BasicName { get; set; }
        //public string Nconst { get; set; }
        //public string PrimaryName { get; set; }
        public string JobName { get; set; }

        public CrewModel ConvertFromDTO(TitleCastDTO inputModl)
        {
            return new CrewModel
            {
                Tconst = inputModl.Tconst,
                BasicName = new BasicNameModel { PrimaryName = inputModl.PrimaryName },
                JobName = inputModl.CharacterName
            };
        }

    }
}
