using demo.Models;
using Demo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BUS.IBUS
{
    public interface IUserBUS
    {
        Task<bool> CreateAsync(CreateUserRequest request);
        List<ResponseUser> GetList();
        ResponseUser Get(int userId);
        bool Update(int userId, UpdateUserRequest userDTO);
        bool Delete(int userId);
        bool Login(LoginRequest request);
    }
}
