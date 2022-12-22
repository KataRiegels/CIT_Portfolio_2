using DataLayer.DTOs.NameObjects;

namespace WebServer.Models.NameModels
{
    public class DetailedNameModel
    {
        public string Url { get; set; }
        public string? PrimaryName { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public IList<string>? KnwonForTconst { get; set; }

        public DetailedNameModel ConvertFromDTO(DetailedNameDTO inputModel)
        {
            return new DetailedNameModel
            {
                BirthYear = inputModel.BirthYear,
                PrimaryName = inputModel.PrimaryName,
                DeathYear = inputModel.DeathYear,
                KnwonForTconst = inputModel.KnwonForTconst,
            };
        }



    }
}
