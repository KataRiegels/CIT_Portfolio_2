﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataTransferObjects;

namespace DataLayer.DataTransferObjects
{
    public class DetailedProducerModel
    {
        public BasicNameModelDL BasicName { get; set; }
        public IList<BasicTitleModelDL> Titles { get; set; }
    }
}