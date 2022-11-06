using Demo.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public GoogleLogin? GoogleLogin { get; set; }
    }
}