using DataLayer.Model;
using WebServer.Models.TitleModels;

namespace WebServer.Models.NameModels
{
    public class ListNameModel
    {
        public BasicNameModel BasicName { get; set; }
        public string Nconst { get; set; }
        public string PrimaryName { get; set; }
        //public string Profession { get; set; }

        //public Tuple<string, BasicTitleModelDL> KnownForTitleBasics { get; set; }
        public BasicTitleModel? KnownForTitleBasics { get; set; } = null;


        //public string KnownForTitle { get; set; }
        //public string StartYear { get; set; }
        //public string TitleType { get; set; }
        //public string Tconst { get; set; }

    }
}
