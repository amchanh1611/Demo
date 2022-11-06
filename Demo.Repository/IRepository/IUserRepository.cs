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
        List<User> GetList();
        User Get(int userID);
        bool Create(User user);
        bool Upadte(User users);
        bool Delete(int userId);
        User Login(string userName);
        User FindByEmail(string email);
        
    }
}
