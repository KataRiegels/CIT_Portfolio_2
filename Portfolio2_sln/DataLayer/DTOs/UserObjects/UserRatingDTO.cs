using DataLayer.DTOs.TitleObjects;

namespace DataLayer.DTOs.UserObjects
{
    public class UserRatingDTO
    {
        public BasicTitleDTO TitleModel { get; set; }
        public int Rating { get; set; }

        public DateTime? Date { get; set; }
    }
}
