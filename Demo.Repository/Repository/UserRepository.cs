using demo.Models;
using Demo.DTO;
using Demo.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demo.Models;
using System.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;

namespace Demo.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DemoDbContext context;
        public UserRepository(DemoDbContext context)
        {
            this.context = context;
        }

        public bool Create(User user)
        {
            var query= context.users.Add(user);
            var check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public bool Delete(int userId)
        {
            var user = context.users.SingleOrDefault(s => s.Id==userId);
            var query = context.Remove(user);
            var check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<User> GetList()
        {
            return context.users.ToList();
        }

        public User Get(int userId)
        {
            return context.users.SingleOrDefault(s => s.Id == userId);
        }

        public bool Login(string userName, string password)
        {
            var query= context.users.SingleOrDefault(s => s.UserName == userName && s.Password == password);
            if (query != null)
                return true;
            return false;
        }

        public bool Upadte(User Request)
        {
            context.Update(Request);
            var check = context.SaveChanges();
            return check > 0 ?true:false;
        }
    }
}
