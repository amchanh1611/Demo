using demo.Models;
using Demo.Models;
using Demo.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repository.Repository
{
    public class LoginGoogleRepository : ILoginGoogleRepository
    {
        private readonly DemoDbContext context;
        public LoginGoogleRepository(DemoDbContext context)
        {
            this.context = context;
        }

        public bool CreateGoogleLogin(GoogleLogin googleLogin)
        {
            context.googleLogins.Add(googleLogin);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public GoogleLogin FindByCondition(string key)
        {
            return context.googleLogins.Where(x => x.Key == key).FirstOrDefault();
        }
    }
}