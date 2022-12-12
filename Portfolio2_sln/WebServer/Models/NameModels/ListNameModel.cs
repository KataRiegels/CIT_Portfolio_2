using DataLayer.DTOs.NameObjects;
using WebServer.Models.TitleModels;

namespace WebServer.Models.NameModels
{
    public class ListNameModel
    {
        public BasicNameModel BasicName { get; set; }
        //public string Nconst { get; set; }
        //public string PrimaryName { get; set; }
        //public string Profession { get; set; }

        //public Tuple<string, BasicTitleModelDL> KnownForTitleBasics { get; set; }
        public BasicTitleModel? KnownForTitleBasics { get; set; } = null;

        public ListNameModel ConvertFromListTitleDTO(ListNameModelDL inputModel)
        {

            //var basic = new BasicTitleModel().ConvertBasicTitleModel(inputModel.BasicTitle);
            return new ListNameModel()
            {
                BasicName = new BasicNameModel().ConvertFromBasicNameModelDTO(inputModel.BasicName),
                KnownForTitleBasics = new BasicTitleModel().ConvertBasicTitleModel(inputModel.KnownForTitleBasics)
            };
        }

        //public string KnownForTitle { get; set; }
        //public string StartYear { get; set; }
        //public string TitleType { get; set; }
        //public string Tconst { get; set; }

    }
}
