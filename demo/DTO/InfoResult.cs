using System.Text.Json.Serialization;

namespace Demo.DTO
{
    public class InfoResult
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
