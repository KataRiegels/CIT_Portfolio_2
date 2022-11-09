using WebServer.Models.NameModels;

namespace WebServer.Models.TitleModels
{
    public class DetailedTitleModel
    {
        public ListTitleModel Title { get; set; }

        public IList<ListNameModel> RelatedNames { get; set; }

        public string Plot { get; set; }
        //public string Poster { get; set; }



    }
}
