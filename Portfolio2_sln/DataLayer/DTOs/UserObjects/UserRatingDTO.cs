using DataLayer.DTOs.TitleObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.UserObjects
{
    public class UserRatingDTO
    {
        public BasicTitleModelDL TitleModel { get; set; }
        public int Rating { get; set; }

        public DateTime? Date { get; set; }
    }
}
