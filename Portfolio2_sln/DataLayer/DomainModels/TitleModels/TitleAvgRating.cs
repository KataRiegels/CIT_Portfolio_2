using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainModels.TitleModels
{
    public class TitleAvgRating
    {
        public string Tconst { get; set; }
        public double AverageRating { get; set; }
        public int NumVotes { get; set; }
    }
}
