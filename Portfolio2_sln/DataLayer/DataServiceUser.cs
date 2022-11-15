using DataLayer.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataServiceUser : IDataServiceUser
    {
        private static ImdbContext _db = new ImdbContext();
        public User GetUser(string username)
        {
            
            return _db.Users.FirstOrDefault(x => x.Username == username);
        }

        public IList<User> GetUsers()
        {
            return _db.Users.ToList();
        }





    }
}
