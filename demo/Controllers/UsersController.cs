using Demo.AppSettings;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Helper.JWT;
using Demo.Helpers;
using Demo.Services;
using Hangfire;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Claims;
using System.Text.Json;
using System.Timers;


namespace Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBUS userBUS;
        private readonly IJwtUtils jwtUtils;
        private readonly GoogleSettings google;
        private readonly EmailConfiguration emailConfig;
        private readonly IUserServies userServices;
        private readonly IBackgroundJobClient backgroundJobClient;

        public UsersController(IUserBUS userBUS, IJwtUtils jwtUtils, IOptions<GoogleSettings> google, IOptions<EmailConfiguration> emailConfig, IUserServies userServices, IBackgroundJobClient backgroundJobClient)
        {
            this.userBUS = userBUS;
            this.jwtUtils = jwtUtils;
            this.google = google.Value;
            this.emailConfig = emailConfig.Value;
            this.userServices = userServices;
            this.backgroundJobClient = backgroundJobClient;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest request)
        {
            bool result = await userBUS.CreateAsync(request);
            if (result)
                return Ok();
            return BadRequest("Eror");
        }

        [HttpGet]
        public ActionResult GetList()
        {
            List<ResponseUser> result = userBUS.GetList();
            if (result != null)
                return Ok(result);
            return BadRequest("Eror");
        }

        [HttpGet("{userId}")]
        public ActionResult Get([FromRoute] int userId)
        {
            HttpContext context = HttpContext;
            ResponseUser result = userBUS.Get(context, userId);
            if (result != null)
                return Ok(result);
            return BadRequest("Eror");
        }

        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            LoginResponse result = userBUS.Login(request);
            if (result != null)
                return Ok(result);
            return BadRequest("Incorrect username/password");
        }

        [HttpPut("{userId}")]
        public ActionResult Upadte([FromRoute] int userId, [FromForm] UpdateUserRequest request)
        {
            bool result = userBUS.Update(userId, request);
            if (result)
                return Ok();
            return BadRequest("Eror");
        }

        [HttpDelete("{userId}")]
        public ActionResult Delete([FromRoute] int userId)
        {
            bool result = userBUS.Delete(userId);
            if (result)
                return Ok();
            return BadRequest("Eror");
        }

        [HttpGet("LoginLink")]
        public IActionResult LoginLink()
        {
            LinkLoginGoogle link = new();

            link.Link = $"{google.IdentityPlatform.AuthUri}&redirect_uri={google.RedirectUri}&client_id={google.ClientId}";
            return Ok(link);
        }

        [HttpPut("Profile"), Authorize]
        public ActionResult UpdateProfile([FromForm] UpdateUserRequest request)
        {
            Claim? claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(claim!.Value);
            bool result = userBUS.Update(userId, request);
            if (result)
                return Ok();
            return BadRequest("Update Fail");
        }

        [HttpGet("LoginGoogle")]
        public async Task<IActionResult> LoginGoogleAsync([FromQuery] string code)
        {
            HttpClient client = new();
            HttpResponseMessage responseToken = await client.PostAsync($"{google.IdentityPlatform.TokenUri}&code={code}&client_id={google.ClientId}&client_secret={google.ClientSecret}&redirect_uri={google.RedirectUri}", new StringContent(""));
            //responseToken.get
            string contentToken = await responseToken.Content.ReadAsStringAsync();
            //TokenResult tokenResult = JsonConvert.DeserializeObject<TokenResult>(contentToken);
            TokenResult tokenResult = JsonSerializer.Deserialize<TokenResult>(contentToken);

            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResult.Access_token}");
            //HttpResponseMessage responseInfo = await client.GetAsync($"{google.IdentityPlatform.UserInfoUri}");

            //HttpResponseMessage responseInfo = await client.GetAsync($"{google.IdentityPlatform.UserInfoUri}?access_token={tokenResult.Access_token}");

            HttpResponseMessage responseInfo = await client.PostAsync($"{google.IdentityPlatform.UserInfoUri}?access_token={tokenResult.AccessToken}", new StringContent(""));
            string contentInfo = await responseInfo.Content.ReadAsStringAsync();
            InfoResult infoResult = JsonSerializer.Deserialize<InfoResult>(contentInfo);
            Random oTP = new();
            var message = new DTO.Message("am.chanh1199@gmail.com", "Test email", $"This is now OTP : {oTP.Next(1000, 9999)}\n Expire 2 minute ");

            MimeMessage mimeMessage = CreateEmailMessage(message, infoResult.Email);
            TimeSpan time = new(0, 0, 10);
            //await userServices.SendMessageAsync(tokenResult.AccessToken, "am.chanh16111@gmail.com", mimeMessage);

            backgroundJobClient.Enqueue( () => SendMessageAsync(tokenResult.AccessToken, "0468191090@caothang.edu.vn",infoResult.Email));

            //int statusCode = await userServices.SendMessageAsync(tokenResult.AccessToken, "0468191090@caothang.edu.vn", mimeMessage);

            return Ok();
        }
        public void Console()
        {
            System.Console.WriteLine("Yes....");
        }

        //[HttpPost("LoginGoogle")]
        //public async Task<IActionResult> LoginGoogleAsync(ExternalAuthDto externalAuthDto)
        //{
        //    Payload payload = await userBUS.VerifyGoogleToken(externalAuthDto);
        //    if (payload == null)
        //        return BadRequest("Invalid External Authentication.");

        //    User user = userBUS.FindByLoginGoogle(payload.Email, externalAuthDto.Provider);
        //    string token = jwtUtils.GenerateJwtToken(user);

        //    if (user != null)
        //        return Ok(token);

        //    bool check = await userBUS.CreateAsync(new CreateUserRequest
        //    {
        //        Email = payload.Email,
        //        UserName = payload.Email,
        //        FullName = payload.Name,
        //        Provider = externalAuthDto.Provider
        //    });
        //    if (check)
        //        return Ok(token);
        //    return BadRequest();

        //}
        [HttpGet("SendMailSmtp")]
        public IActionResult SendMailSmtp([FromQuery] string email)
        {
            Random oTP = new();
            var message = new DTO.Message(email, "Test email", $"OTP : {oTP.Next(1000, 9999)}");

            MimeMessage mimeMessage = CreateEmailMessage(message, emailConfig.From);

            SmtpClient client = new();
            client.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2"); // Các cơ chế xác thực được truy vấn như một phần của quá trình kết nối. Để ngăn việc sử dụng các cơ chế xác thực nhất định
            client.Authenticate(emailConfig.Username, emailConfig.Password);
            client.Send(mimeMessage);
            return Ok();
        }
        private MimeMessage CreateEmailMessage(DTO.Message message, string emailFrom)
        {
            MimeMessage emailMessage = new();
            emailMessage.From.Add(new MailboxAddress("Test ", emailFrom));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        public async Task SendMessageAsync(string senderAccessToken, string senderEmail,string sendToEmail)
        {
            Random oTP = new();
            var message = new DTO.Message("am.chanh1199@gmail.com", "Test email", $"This is now OTP : {oTP.Next(1000, 9999)}\n Expire 2 minute ");

            MimeMessage mimeMessage = CreateEmailMessage(message, sendToEmail);
            object dataRequestSendMessage = new
            {
                raw = mimeMessage.Base64UrlSafeEncode()
            };

            HttpClient client = new();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {senderAccessToken}");

            HttpResponseMessage response = await client.PostAsync($"{google.Gmail.UsersUri}/me/messages/send", new StringContent(JsonSerializer.Serialize(dataRequestSendMessage)));
            System.Console.WriteLine(response.StatusCode);


        }

    }

}