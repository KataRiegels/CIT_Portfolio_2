using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class DetailedTitleModelDL
    {
        public string Tconst { get; set; }
        public string PrimaryTitle { get; set; }
        public string startyear { get; set; }
        public string? titletype { get; set; }
        public int? runtime { get; set; }
        public double? rating { get; set; }
        public IList<string>? genre { get; set; }
        public string? plot { get; set; }
        public string? poster { get; set; }
        public string? relatedName { get; set; }

    }
}
