using demo.Models;
using Demo.DTO;
using Microsoft.AspNetCore.Http;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Demo.BUS.IBUS
{
    public interface IUserBUS
    {
        Task<bool> CreateAsync(CreateUserRequest request);

        List<ResponseUser> GetList();

        ResponseUser Get(HttpContext context,int userId);

        bool Update(int userId, UpdateUserRequest userDTO);

        bool Delete(int userId);

        LoginResponse Login(LoginRequest request);
        Task<Payload> VerifyGoogleToken(string clientId, string token);
        User FindByLoginGoogle(string email, string provider);
    }
}