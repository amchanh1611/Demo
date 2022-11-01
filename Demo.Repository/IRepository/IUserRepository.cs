using demo.Models;
using Demo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Repository.IRepository
{
    public interface IUserRepository
    {
        public List<User> GetList();
        public User Get(int userID);
        public bool Create(User user);
        public bool Upadte(User users);
        public bool Delete(int userId);
        public User Login(string userName);
    }
}
