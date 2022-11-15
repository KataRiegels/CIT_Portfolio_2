using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class SearchResult
    {
        public IList<SearchNameModel>? NameResults { get; set; }
        public IList<SearchTitleModel>? TitleResults { get; set; }
    }
}
