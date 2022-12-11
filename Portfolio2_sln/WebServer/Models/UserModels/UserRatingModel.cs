using DataLayer.DataTransferObjects;
using DataLayer.Models.UserModels;
using WebServer.Models.TitleModels;

namespace WebServer.Models.UserModels
{
    public class UserRatingModel
    {

        public BasicTitleModel TitleModel { get; set; }
        public int Rating { get; set; }


        public UserRatingModel ConvertFromDTO(UserRatingDTO inputModel)
        {

            return new UserRatingModel { 
                Rating = inputModel.Rating,
                TitleModel = new BasicTitleModel().ConvertBasicTitleModel(inputModel.TitleModel),
            };
        }
    }
}
