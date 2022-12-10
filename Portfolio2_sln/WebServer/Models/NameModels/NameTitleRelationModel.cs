using DataLayer.DataTransferObjects;
using WebServer.Models.TitleModels;

namespace WebServer.Models.NameModels
{
    public class NameTitleRelationModel
    {
        public string Nconst { get; set; }
        public BasicTitleModel Title { get; set; }
        public string Relation { get; set; }
    
        public NameTitleRelationModel ConvertFromDTO(NameTitleRelationDTO inputModel)
        {
            return new NameTitleRelationModel
            {
                Nconst = inputModel.Nconst,
                Relation = inputModel.Relation,
                Title = new BasicTitleModel().ConvertBasicTitleModel(inputModel.Title)
            };
        }

    }
}
