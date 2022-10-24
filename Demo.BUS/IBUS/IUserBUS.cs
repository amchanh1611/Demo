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
        public bool Create(CreateUserRequest userDTO);
        public List<ResponseUser> GetList();
        public ResponseUser Get(int userId);
        public bool Update(int userId, UpdateUserRequest userDTO);
        public bool Delete(int userId);
        public bool Login(LoginRequest request);
    }
}
