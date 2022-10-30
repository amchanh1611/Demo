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

        public async Task<bool> CreateAsync(HttpContext context, CreateUserRequest request)
        {
            User user = mapper.Map<User>(request);

            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar", request.FormFile.FileName);

            string relativePath = Path.Combine((context.Request.Host).ToString(), "wwwroot/Avatar", request.FormFile.FileName);
            user.Avatar = relativePath;
            using (Stream stream = new FileStream(absolutePath, FileMode.Create))
            {
                await request.FormFile.CopyToAsync(stream);
            }
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

        public ResponseUser Get(int userId)
        {
            User user = userRepository.Get(userId);
            ResponseUser response = mapper.Map<ResponseUser>(user);
            //var content = new System.IO.MemoryStream(user.Avatar);
            var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "wwwroot",
                  user.Avatar);
            //using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            //{
            //    content.CopyToAsync(fileStream);
            //}
            MemoryStream memoryStream = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0;
            //response.FormFile = IFormFile()
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