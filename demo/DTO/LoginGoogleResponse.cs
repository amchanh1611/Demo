namespace Demo.DTO
{
    public class LoginGoogleResponse
    {
        public LoginGoogleResponse()
        {

        }
        public LoginGoogleResponse(string jwtToken,string refreshToken)
        {
            JwtToken=jwtToken;
            RefreshToken=refreshToken;
        }
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
