using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferObjects
{
    public class DetailedActorModel
    {
        public BasicNameModelDL BasicName { get; set; }
        public string CharacterName { get; set; }
        public IList<BasicTitleModelDL> Titles { get; set; }
    }
}
