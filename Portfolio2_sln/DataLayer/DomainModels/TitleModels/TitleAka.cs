﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.TitleModels
{
    public class TitleAka
    {
        public string Tconst { get; set; }
        public int Ordering { get; set; }
        public string Title { get; set; }
        public string Region { get; set; }
        public string Language { get; set; }
        public string Types { get; set; }
        public string Attributes { get; set; }
        //public string  { get; set; }
        public bool IsOriginalTitle { get; set; }
    }
}
