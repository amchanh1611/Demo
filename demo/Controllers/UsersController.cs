using Demo.AppSettings;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Helper.JWT;
using Google.Apis.Gmail.v1.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using System.Security.Claims;
using static Google.Apis.Auth.GoogleJsonWebSignature;


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

        public UsersController(IUserBUS userBUS, IJwtUtils jwtUtils, IOptions<GoogleSettings> google, IOptions<EmailConfiguration> emailConfig)
        {
            this.userBUS = userBUS;
            this.jwtUtils = jwtUtils;
            this.google = google.Value;
            this.emailConfig = emailConfig.Value;
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
            TokenResult tokenResult = JsonConvert.DeserializeObject<TokenResult>(contentToken);

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResult.Access_token}");
            //HttpResponseMessage responseInfo = await client.GetAsync($"{google.IdentityPlatform.UserInfoUri}");

            //HttpResponseMessage responseInfo = await client.GetAsync($"{google.IdentityPlatform.UserInfoUri}?access_token={tokenResult.Access_token}");

            HttpResponseMessage responseInfo = await client.PostAsync($"{google.IdentityPlatform.UserInfoUri}?access_token={tokenResult.Access_token}", new StringContent(""));



            string contentInfo = await responseInfo.Content.ReadAsStringAsync();
            return Ok();
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
            var message = new DTO.Message(email, "Test email", $"OTP : {oTP.Next(1000,9999)}");

            MimeMessage mimeMessage = CreateEmailMessage(message);

            SmtpClient client = new();
            client.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2"); // Các cơ chế xác thực được truy vấn như một phần của quá trình kết nối. Để ngăn việc sử dụng các cơ chế xác thực nhất định
            client.Authenticate(emailConfig.Username, emailConfig.Password);
            client.Send(mimeMessage);
            return Ok();
        }
        private MimeMessage CreateEmailMessage(DTO.Message message)
        {
            MimeMessage emailMessage = new();
            emailMessage.From.Add(new MailboxAddress("Test SMTP",emailConfig.From));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
       
    }
    
}