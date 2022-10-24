using AutoMapper;
using Demo.DTO;
using demo.Models;
namespace Demo.Helper.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        { 
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, ResponseUser>();
        }
    }
}