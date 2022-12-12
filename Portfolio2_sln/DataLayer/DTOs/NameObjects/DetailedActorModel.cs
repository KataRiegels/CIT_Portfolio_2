using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DTOs.NameObjects
{
    public class DetailedActorModel
    {
        public BasicNameModelDL BasicName { get; set; }
        public string CharacterName { get; set; }
        public IList<BasicTitleModelDL> Titles { get; set; }
    }
}
