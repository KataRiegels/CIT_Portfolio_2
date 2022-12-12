using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DTOs.NameObjects
{
    public class NameTitleRelationDTO
    {
        public string Nconst { get; set; }
        public BasicTitleDTO Title { get; set; }
        public string Relation { get; set; }




    }
}
