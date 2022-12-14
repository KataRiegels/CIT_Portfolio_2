using DataLayer.DTOs.UserObjects;
using WebServer.Models.TitleModels;

namespace WebServer.Models.UserModels
{
    public class UserRatingModel
    {

        public string? Url { get; set; }
        public BasicTitleModel? TitleModel { get; set; }
        public int? Rating { get; set; }


        public UserRatingModel ConvertFromDTO(UserRatingDTO inputModel)
        {

            return new UserRatingModel
            {
                Rating = inputModel.Rating,
                TitleModel = new BasicTitleModel().ConvertFromDTO(inputModel.TitleModel),
            };
        }
    }
}
