using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DTOs.NameObjects;
using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DTOs.SearchObjects
{
    public class SearchResult
    {
        public int SearchId { get; set; }
        public IList<ListNameModelDL>? NameResults { get; set; }
        public IList<ListTitleModelDL>? TitleResults { get; set; }

        //public IList<SearchNameModel>? NameResults { get; set; }
        //public IList<SearchTitleModel>? TitleResults { get; set; }
    }
}
