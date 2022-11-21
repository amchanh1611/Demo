using demo.Models;
using Demo.AppSettings;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Helper.JWT;
using Demo.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;

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
            return Ok($"{google.IdentityPlatform.AuthUri}?scope=email%20profile%20openid%20https://www.googleapis.com/auth/gmail.send&access_type=offline&include_granted_scopes=true&response_type=code&redirect_uri={google.RedirectUri}&client_id={google.ClientId}");
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
            using HttpClient client = new();
            HttpResponseMessage responseToken = await client.PostAsync($"{google.IdentityPlatform.TokenUri}?grant_type=authorization_code&code={code}&client_id={google.ClientId}&client_secret={google.ClientSecret}&redirect_uri={google.RedirectUri}", new StringContent(""));

            string contentToken = await responseToken.Content.ReadAsStringAsync();
            TokenResult tokenResult = JsonSerializer.Deserialize<TokenResult>(contentToken);

            HttpResponseMessage responseInfo = await client.PostAsync($"{google.IdentityPlatform.UserInfoUri}?access_token={tokenResult.AccessToken}", new StringContent(""));
            string contentInfo = await responseInfo.Content.ReadAsStringAsync();

            InfoResult infoResult = JsonSerializer.Deserialize<InfoResult>(contentInfo);

            User user = new();
            string token;
            user = userBUS.FindByLoginGoogle(infoResult.Email, "google");

            if (user != null)
            {
                token = jwtUtils.GenerateJwtToken(user);
                return Ok(token);
            }

            bool check = await userBUS.CreateAsync(new CreateUserRequest
            {
                Email = infoResult.Email,
                UserName = infoResult.Email,
                FullName = $"{infoResult.LastName} {infoResult.FirstName}",
                RefreshToken = tokenResult.RefreshToken,
                Provider = "google"
            });
            user = userBUS.FindByLoginGoogle(infoResult.Email, "google");
            token = jwtUtils.GenerateJwtToken(user);
            return Ok(token);
        }

        [HttpPost("SendMail/{userId}")]
        public IActionResult SendMail([FromRoute] int userId, [FromBody] MessageRequest message)
        {
            DateTimeOffset date = new(message.DateSend!.Value);
            backgroundJobClient.Schedule(() => userServices.SendMessageAsync(userId, message), date);
            //userServices.SendMessageAsync(userId, message);
            return Ok();
        }

        //[HttpGet("SendMailSmtp")]
        //public IActionResult SendMailSmtp([FromQuery] string email)
        //{
        //    Random oTP = new();
        //    var message = new DTO.Message(email, "Test email", $"OTP : {oTP.Next(1000, 9999)}");

        //    MimeMessage mimeMessage = CreateEmailMessage(message, emailConfig.From);

        //    SmtpClient client = new();
        //    client.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
        //    client.AuthenticationMechanisms.Remove("XOAUTH2"); // Các cơ chế xác thực được truy vấn như một phần của quá trình kết nối. Để ngăn việc sử dụng các cơ chế xác thực nhất định
        //    client.Authenticate(emailConfig.Username, emailConfig.Password);
        //    client.Send(mimeMessage);
        //    return Ok();
        //}
    }
}