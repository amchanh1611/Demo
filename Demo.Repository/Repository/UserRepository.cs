using demo.Models;
using Demo.Repository.IRepository;

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
            context.users.Add(user);
            var check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public bool Delete(int userId)
        {
            var user = context.users.SingleOrDefault(s => s.Id == userId);
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

        public User Login(string userName)
        {
            return context.users.SingleOrDefault(s => s.UserName == userName);
        }

        public bool Upadte(User Request)
        {
            context.Update(Request);
            var check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public User FindByEmail(string email)
        {
            return context.users.Where(x => x.Email == email).FirstOrDefault();
        }
    }
}