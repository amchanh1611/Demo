using Microsoft.AspNetCore.Http;

namespace Demo.DTO
{
    public class CreateUserRequest : BaseUserRequest
    {
        public IFormFile Avatar { get; set; }
    }
}