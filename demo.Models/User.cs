﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Byte[]? Avatar { get; set; }
    }
}