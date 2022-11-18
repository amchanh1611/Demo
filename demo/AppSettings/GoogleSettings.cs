namespace Demo.AppSettings
{
    public class GoogleSettings
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? RedirectUri { get; set; }
        public IdentityPlatform? IdentityPlatform { get; set; }
        public Gmail? Gmail { get; set; }
    }

    public class IdentityPlatform
    {
        public string? AuthUri { get; set; }

        public string? TokenUri { get; set; }

        public string? UserInfoUri { get; set; }
    }
    public class Gmail
    {
        public string? UsersUri { get; set; }
    }
}
