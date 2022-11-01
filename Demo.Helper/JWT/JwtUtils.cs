using demo.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Helper.JWT
{
    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings appSettings;

        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://minhchanh.com",
                Audience = user.Id.ToString(),
                Subject = new ClaimsIdentity(new[]
                { 
                    //new Claim("id", user.Id.ToString()),
                    new Claim("userName", user.UserName),
                    new Claim("email",user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}