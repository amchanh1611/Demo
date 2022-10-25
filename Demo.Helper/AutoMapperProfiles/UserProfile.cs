using AutoMapper;
using Demo.DTO;
using demo.Models;
using BC = BCrypt.Net.BCrypt;
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
    public interface IValueResolver<in TSource, in TDestination, TDestMember>
    {
        TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember, ResolutionContext context);
    }
    public class RequestMappingUser : IValueResolver<CreateUserRequest, User, string>
    {
        public string Resolve(CreateUserRequest source, User destination, string destMember, ResolutionContext context)
        {
            return source.Password = BC.HashPassword(source.Password);
        }
    }
}