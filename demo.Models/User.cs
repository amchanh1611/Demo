namespace demo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public DateTime? Birtday { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Avatar { get; set; }
        public string Provider { get; set; } = default!;
        //public string? ProviderKey { get; set; }
    }
}