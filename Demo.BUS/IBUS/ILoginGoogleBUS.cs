using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BUS.IBUS
{
    public interface ILoginGoogleBUS
    {
        bool Create(GoogleLogin googleLogin);
        GoogleLogin FindByCondition(string key);
    }
}
