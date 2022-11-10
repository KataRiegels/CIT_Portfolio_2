using DataLayer.Model;

namespace WebServer.Models.NameModels
{
    public class ListNameModel
    {
        public BasicNameModel BasicName { get; set; }
        public string Occupation { get; set; }
        public BasicTitleModelDL KnownForTitle { get; set; }


    }
}
