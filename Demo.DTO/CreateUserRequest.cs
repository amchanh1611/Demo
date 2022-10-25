namespace Demo.DTO
{
    public class CreateUserRequest : BaseUserRequest
    {
        public IFormFile FormFile { get; set; }
    }
}