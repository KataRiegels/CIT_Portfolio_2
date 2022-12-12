using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DTOs.TitleObjects;






namespace DataLayer.DTOs.NameObjects
{
    public class NameForListDTO
    {

        public BasicNameDTO BasicName { get; set; }

        //public string Nconst { get; set; }
        //public string PrimaryName { get; set; }
        //public string Profession { get; set; }

        //public Tuple<string, BasicTitleDTO> KnownForTitleBasics { get; set; }
        //public BasicTitleDTO KnownForTitleBasics { get; set; }
        public BasicTitleDTO? KnownForTitleBasics { get; set; }
        //public string? KnownForTitle { get; set; }
        //public string? StartYear { get; set; }
        //public string? TitleType { get; set; }
        //public string? Tconst { get; set; }


    }
}
