using Demo.AppSettings;
using Demo.Helpers;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demo.Services
{
    public interface IUserServies
    {
        Task SendMessageAsync(string senderAccessToken, string senderEmail, MimeMessage mimeMessage);
    }
    public class UserServies : IUserServies
    {
        private readonly GoogleSettings google;
        public UserServies(IOptions<GoogleSettings> google)
        {
            this.google = google.Value;
        }
        public async Task SendMessageAsync(string senderAccessToken, string senderEmail, MimeMessage mimeMessage)
        {
            object dataRequestSendMessage = new
            {
                raw = mimeMessage.Base64UrlSafeEncode()
            };
            
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {senderAccessToken}");

            HttpResponseMessage response = await client.PostAsync($"{google.Gmail.UsersUri}/me/messages/send", new StringContent(JsonSerializer.Serialize(dataRequestSendMessage)));

            Console.WriteLine("SendMailSuccess");
                
        }
    }
}
