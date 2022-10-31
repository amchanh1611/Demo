using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DTO
{
    public class ResponseUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public DateTime? Birtday { get; set; }
        public string? Email { get; set; }
        public int? Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Avatar { get; set; }
    }
}
