using demo.Models;
using Demo.AppSettings;
using Demo.Common.Extentions;
using Demo.DTO;
using Demo.Repository.IRepository;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.Json;

namespace Demo.Services
{
    public interface IUserServies
    {
        Task SendMessageAsync(int userId, MessageRequest message);
    }

    public class UserServies : IUserServies
    {
        private readonly GoogleSettings google;
        private readonly EmailConfiguration email;
        private readonly IUserRepository userRepository;

        public UserServies(IOptions<GoogleSettings> google, IOptions<EmailConfiguration> email, IUserRepository userRepository)
        {
            this.google = google.Value;
            this.email = email.Value;
            this.userRepository = userRepository;
        }

        public async Task SendMessageAsync(int userId, MessageRequest messageRequest)
        {
            User user = userRepository.Get(userId);

            HttpClient client = new();
            HttpResponseMessage responseRefresh = await client.PostAsync($"{google.IdentityPlatform.TokenUri}?client_id={google.ClientId}&client_secret={google.ClientSecret}&refresh_token={user.RefreshToken}&grant_type=refresh_token", new StringContent(""));
            string contentRefresh = await responseRefresh.Content.ReadAsStringAsync();
            TokenResult tokenResult = JsonSerializer.Deserialize<TokenResult>(contentRefresh);

            Message message = new(messageRequest.To, messageRequest.Subject, messageRequest.Content);
            MimeMessage mimeMessage = message.CreateEmailMessage(email.From);

            object dataRequestSendMessage = new
            {
                raw = mimeMessage.Base64UrlSafeEncode()
            };

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResult.AccessToken}");

            HttpResponseMessage response = await client.PostAsync($"{google.Gmail.UsersUri}/me/messages/send", new StringContent(JsonSerializer.Serialize(dataRequestSendMessage)));
        }
    }
}