using Demo.BUS.IBUS;
using Demo.Models;
using Demo.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BUS.BUS
{
    public class LoginGoogleBUS : ILoginGoogleBUS
    {
        private readonly ILoginGoogleRepository loginGoogleRepository;
        public LoginGoogleBUS(ILoginGoogleRepository loginGoogleRepository)
        {
            this.loginGoogleRepository = loginGoogleRepository;
        }
        public bool Create(GoogleLogin googleLogin)
        {
            return loginGoogleRepository.CreateGoogleLogin(googleLogin);
        }

        public GoogleLogin FindByCondition(string key)
        {
            return loginGoogleRepository.FindByCondition(key);
        }
    }
}
