using AutoMapper;
using demo.Models;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;

namespace Demo.BUS.BUS
{
    public class UserBUS : IUserBUS
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UserBUS(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<bool> CreateAsync(CreateUserRequest request)
        {
            User user = mapper.Map<User>(request);
            user.Avatar = await Helper.Helper.UploadFilesAsync(request.FormFile);
            user.CreatedDate = DateTime.UtcNow;
            return userRepository.Create(user);
        }

        public bool Delete(int userId)
        {
            return userRepository.Delete(userId);
        }

        public List<ResponseUser> GetList()
        {
            List<User> lstUser = userRepository.GetList();
            List<ResponseUser> lstResponse = mapper.Map<List<User>, List<ResponseUser>>(lstUser);
            return lstResponse;
        }

        public ResponseUser Get(HttpContext context,int userId)        
        {
            User user = userRepository.Get(userId);
            ResponseUser response = mapper.Map<ResponseUser>(user);
            string path = $"{context.Request.Scheme}://{context.Request.Host}/{user.Avatar}";
            response.Avatar = path;
            return response;
        }

        public bool Login(LoginRequest request)
        {
            return userRepository.Login(request.UserName, request.Password);
        }

        public bool Update(int userId, UpdateUserRequest request)
        {
            User user = userRepository.Get(userId);
            request.Password = BC.HashPassword(request.Password);
            user = mapper.Map(request, user);
            return userRepository.Upadte(user);
        }
        
    }
}