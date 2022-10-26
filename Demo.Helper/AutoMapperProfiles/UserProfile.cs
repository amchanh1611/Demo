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
            //.ForMember(dest => dest.Avatar,src => src.MapFrom(m=>m.Avatar)).ConvertUsing(new FileTypeConverter());
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, ResponseUser>();
        }
        //public class FileTypeConverter : ITypeConverter<IFormFile, byte[]>
        //{
        //    public byte[] Convert(IFormFile source, byte[] destination, ResolutionContext context)
        //    {
        //        MemoryStream memory = new MemoryStream();
        //        source.CopyTo(memory);
        //        return memory.ToArray();
        //    }
        //}
    }
}