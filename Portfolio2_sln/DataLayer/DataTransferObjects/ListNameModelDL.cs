﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferObjects
{
    public class ListNameModelDL
    {

        public BasicNameModelDL BasicName { get; set; }

        //public string Nconst { get; set; }
        //public string PrimaryName { get; set; }
        //public string Profession { get; set; }

        //public Tuple<string, BasicTitleModelDL> KnownForTitleBasics { get; set; }
        //public BasicTitleModelDL KnownForTitleBasics { get; set; }
        public BasicTitleModelDL? KnownForTitleBasics { get; set; }
        //public string? KnownForTitle { get; set; }
        //public string? StartYear { get; set; }
        //public string? TitleType { get; set; }
        //public string? Tconst { get; set; }


    }
}