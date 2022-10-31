using AutoMapper;
using demo.Models;
using Demo.DTO;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;
namespace Demo.Helper.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Password, src => src.MapFrom(m => BC.HashPassword(m.Password)));
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, ResponseUser>();
        }

    }
}