using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.TitleObjects
{
    public class DetailedTitleDTO
    {

        public string Tconst { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? StartYear { get; set; }
        public string? TitleType { get; set; }
        public int? Runtime { get; set; }
        public double? Rating { get; set; }
        public IList<string>? Genres { get; set; }
        public string? Plot { get; set; }
        public string? Poster { get; set; }
        public string? relatedName { get; set; }

    }
}
