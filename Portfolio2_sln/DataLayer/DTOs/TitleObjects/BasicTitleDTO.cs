﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.TitleObjects
{
    public class BasicTitleDTO
    {
        public string Tconst { get; set; }
        public string? TitleType { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? StartYear { get; set; }
    }
}