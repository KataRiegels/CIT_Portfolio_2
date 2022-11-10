using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class ListNameModelDL
    {
        public BasicNameModelDL BasicName { get; set; }
        public string Occupation { get; set; }
        public BasicTitleModelDL KnownForTitle { get; set; }
    }
}
