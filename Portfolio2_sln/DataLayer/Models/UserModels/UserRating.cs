using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.UserModels
{
    public class UserRating
    {
        public string Username { get; set; }
        public string Tconst { get; set; }
        public int Rating { get; set; }
        public DateTime? Date { get; set; }

    }
}
