using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class NamesForTitleModel
    {
        public string Nconst { get; set; }
        public string? PrimaryName { get; set; }
        public string Tconst { get; set; }

        public string Category { get; set; }
        public string? Job { get; set; }
        public IList<string>? Characters { get; set; }
        //public IList<T,T>? Characters { get; set; }
        //public IList<KeyValuePair<string,string>>? Characters { get; set; }

    }
}
