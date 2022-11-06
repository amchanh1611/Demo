using Demo.Models;

namespace Demo.Repository.IRepository
{
    public interface ILoginGoogleRepository
    {
        bool CreateGoogleLogin(GoogleLogin googleLogin);
        GoogleLogin FindByCondition(string key);
    }
}