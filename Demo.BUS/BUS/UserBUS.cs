using AutoMapper;
using demo.Models;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Repository.IRepository;
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

        public bool Create(CreateUserRequest request)
        {
            request.Password = BC.HashPassword(request.Password);
            User user = mapper.Map<User>(request);
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

            //foreach (var user in lstUser)
            //{
            //    lstResponse.Add(mapper.Map<ResponseUser>(user));
            //}
            return lstResponse;
        }

        public ResponseUser Get(int userId)
        {
            User user = userRepository.Get(userId);
            ResponseUser response = mapper.Map<ResponseUser>(user);
            return response;
        }

        public bool Login(LoginRequest request)
        {
            return userRepository.Login(request.UserName, request.Password);
        }

        public bool Update(int userId, UpdateUserRequest request)
        {
            User user = userRepository.Get(userId); 
            request.Password=BC.HashPassword(request.Password);
            user = mapper.Map(request, user);
            return userRepository.Upadte(user);
        }
    }
}