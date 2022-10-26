using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DTO
{
    public class UpdateUserRequest : BaseUserRequest
    {
        public Byte[]? Avatar { get; set; }
    }
}
