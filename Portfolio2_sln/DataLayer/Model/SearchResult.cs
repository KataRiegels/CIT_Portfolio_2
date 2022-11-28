using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class SearchResult
    {
        public IList<ListNameModelDL>? NameResults { get; set; }
        public IList<ListTitleModelDL>? TitleResults { get; set; }

        //public IList<SearchNameModel>? NameResults { get; set; }
        //public IList<SearchTitleModel>? TitleResults { get; set; }
    }
}
