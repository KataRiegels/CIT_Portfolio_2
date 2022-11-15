using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.TitleModels
{
    public class TitleBasics
    {

        public string Tconst { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }

        //public IList<TitleBasics> Episodes { get; set; }

        //public IList<TitleGenre> TitleGenres { get; set; } = new List<TitleGenre>();

    }
}
