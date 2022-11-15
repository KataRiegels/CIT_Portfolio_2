using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using DataLayer.Models.NameModels;
using DataLayer.Models.TitleModels;
using DataLayer.Models.UserModels;

namespace DataLayer
{
    public interface IDataServiceUser
    {

        public User GetUser(string username);
        public IList<User> GetUsers();


    }
}
