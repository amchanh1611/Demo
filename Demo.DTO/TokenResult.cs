using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DTO
{
    public class TokenResult
    {
        public string? Access_token { get; set; }
        public string? Expires_in { get; set; }
        public string? Scope { get; set; }
        public string? Token_type { get; set; }
    }
}
