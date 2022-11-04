using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.TitleModels
{
    public class TitleAvgRating
    {
        public string Tconst { get; set; }
        public int AverageRating { get; set; }
        public int NumVotes { get; set; }
    }
}
