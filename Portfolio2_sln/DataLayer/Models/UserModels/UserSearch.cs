using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.UserModels
{
    public class UserSearch
    {
        //public string UserId { get; set; }
        public string? Username { get; set; }
        
        public int SearchId { get; set; }
        public DateTime? Date { get; set; }
        public string? SearchContent { get; set; }
        public string? SearchCategory { get; set; }




    }
}
