using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.TitleObjects
{
    public class TitleCrewDTO
    {
        public string Tconst { get; set; }
        public BasicTitleDTO BasicTitle { get; set; }
        public string Nconst { get; set; }
        public string PrimaryName { get; set; }
        public string Category { get; set; }
        public string CharacterName { get; set; }
        public string JobName { get; set; }

    }
}
