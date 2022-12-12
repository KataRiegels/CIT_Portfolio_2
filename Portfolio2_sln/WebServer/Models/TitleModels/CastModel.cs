using DataLayer.DTOs.TitleObjects;
using WebServer.Models.NameModels;

namespace WebServer.Models.TitleModels
{
    public class CastModel
    {
        public string Tconst { get; set; }
        public BasicNameModel BasicName { get; set; }
        //public string Nconst { get; set; }
        //public string PrimaryName { get; set; }
        public string CharacterName { get; set; }

        public CastModel ConvertFromDTO(TitleCastDTO inputModl)
        {
            return new CastModel { 
                Tconst = inputModl.Tconst,
                BasicName = new BasicNameModel { PrimaryName = inputModl.PrimaryName },
                CharacterName = inputModl.CharacterName
            };
        }

    }
}
