using Demo.AppSettings;
using Demo.BUS.IBUS;
using Demo.DTO;
using Demo.Helper.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBUS userBUS;
        private readonly IJwtUtils jwtUtils;
        private readonly GoogleSettings google;

        public UsersController(IUserBUS userBUS, IJwtUtils jwtUtils, IOptions<GoogleSettings> google)
        {
            this.userBUS = userBUS;
            this.jwtUtils = jwtUtils;
            this.google = google.Value;
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
            link.Link = $"{google.IdentityPlatform.AuthUri}&{google.RedirectUri}&{google.ClientId}";
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
        public async Task<IActionResult> LoginGoogleAsync()
        {
            StringContent content = new StringContent("");
            HttpClient client = new();
            //HttpResponseMessage response = await client.PostAsync($"{google.IdentityPlatform.AuthUri}&{google.RedirectUri}&{google.ClientId}",content);
            HttpResponseMessage response = await client.GetAsync("https://accounts.google.com/o/oauth2/v2/auth?scope=https%3A//www.googleapis.com/auth/drive.metadata.readonly&access_type=offline&include_granted_scopes=true&response_type=code&redirect_uri=http://localhost:9000/api/Users/LoginGoogle&client_id=634932227060-2cdka612v49ginvt7mq7no4v79m5d80r.apps.googleusercontent.com");

            

            var location = response.Headers.Location;
            

            if (response.IsSuccessStatusCode)
            {
                foreach (var hearder in response.Headers)
                {
                    Console.WriteLine(hearder);
                }
            }
            if (response.Headers != null)
                return Ok(response.Headers);
            return BadRequest();
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
    }
}