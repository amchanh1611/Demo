﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DTO
{
    public class BaseUserRequest
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public int? Phone { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
