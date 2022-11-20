using System.Text.Json.Serialization;

namespace Demo.DTO
{
    public class InfoResult
    {
        [JsonPropertyName("given_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("family_name")]
        public string? LastName { get; set; }

        [JsonPropertyName("picture")]
        public string? Avatar { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        
    }
}
