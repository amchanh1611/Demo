﻿using demo.Models;

namespace Demo.Helper.JWT
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
    }
}