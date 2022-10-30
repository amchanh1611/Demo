using Demo.DTO;
using Microsoft.AspNetCore.Http;

namespace Demo.BUS.IBUS
{
    public interface IUserBUS
    {
        Task<bool> CreateAsync(HttpContext context, CreateUserRequest request);

        List<ResponseUser> GetList();

        ResponseUser Get(int userId);

        bool Update(int userId, UpdateUserRequest userDTO);

        bool Delete(int userId);

        bool Login(LoginRequest request);
    }
}